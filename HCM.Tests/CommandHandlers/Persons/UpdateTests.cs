using HCM.Domain.Identity;
using HCM.Domain.Persons;
using HCM.Features.Persons.Update;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;

namespace HCM.Tests.CommandHandlers.Persons;

public sealed class UpdateTests
{
    [Fact]
    public async Task UpdatePerson_UpdatesValues()
    {
        await using var context = App.CreateContext();
        var hasher = new PasswordHasher<Person>();
        var person = Person.Create("John","Doe","john@example.com","Dev",100m,"IT",ApplicationRoles.Employee,hasher,"pass");
        context.Persons.Add(person);
        await context.SaveChangesAsync();
        var handler = new UpdatePersonCommandHandler(context, NullLogger<UpdatePersonCommandHandler>.Instance);
        var request = new UpdatePersonRequest
        {
            PersonId = person.Id,
            FirstName = "Jane",
            LastName = "Smith",
            Email = "john@example.com",
            JobTitle = "Dev",
            Salary = 200m,
            Department = "IT",
            Role = ApplicationRoles.Manager
        };

        var result = await handler.Handle(new UpdatePersonCommand(request, ApplicationRoles.HrAdmin), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal("Jane", (await context.Persons.FindAsync(person.Id))!.FirstName);
    }
}