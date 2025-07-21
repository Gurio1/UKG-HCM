using HCM.Domain.Persons;
using HCM.Infrastructure;
using HCM.Shared.Persons;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace HCM.Tests.Shared.Persons;
public sealed class PersonGetterTests
{
    private Person CreatePerson(Guid id)
    {
        var passwordHasher = new Mock<IPasswordHasher<Person>>();
        passwordHasher.Setup(x => x.HashPassword(It.IsAny<Person>(), It.IsAny<string>()))
            .Returns("hashed");

        var person = Person.Create(
            firstName: "Alice",
            lastName: "Smith",
            email: "alice@ex.com",
            jobTitle: "HR",
            salary: 1000,
            department: "HR",
            role: "Employee",
            passwordHasher: passwordHasher.Object,
            password: "dummy"
        );

        typeof(Person).GetProperty(nameof(Person.Id))!.SetValue(person, id);
        
        return person;
    }
    private ApplicationDbContext GetDbContextWithPerson(Guid id)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(options);

        var person = CreatePerson(id);

        context.Persons.Add(person);
        context.SaveChanges();
        
        return context;
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsPerson_WhenPersonExists()
    {
        var personId = Guid.NewGuid();
        var context = GetDbContextWithPerson(personId);

        var getter = new PersonGetter(context, NullLogger<PersonGetter>.Instance);

        var result = await getter.GetByIdAsync(personId);

        Assert.True(result.IsSuccess, "Expected IsSuccess to be true");
        Assert.NotNull(result.Value);
        Assert.Equal(personId, result.Value.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsError_WhenPersonDoesNotExist()
    {
        var context = GetDbContextWithPerson(Guid.NewGuid()); 
        var getter = new PersonGetter(context, NullLogger<PersonGetter>.Instance);

        var result = await getter.GetByIdAsync(Guid.NewGuid()); 

        Assert.True(result.IsFailure, "Expected IsFailure to be true");
    }

    [Fact]
    public async Task GetByIdAsync_LogsErrorAndReturnsFailure_OnException()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .Options; 

        var context = new ApplicationDbContext(options);

        var loggerMock = new Mock<ILogger<PersonGetter>>();
        var getter = new PersonGetter(context, loggerMock.Object);

        var result = await getter.GetByIdAsync(Guid.NewGuid());

        Assert.True(result.IsFailure, "Expected IsFailure to be true");

        loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error getting person by id")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!),
            Times.Once);
    }
}
