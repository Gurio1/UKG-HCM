

using HCM.Domain.Identity;
using HCM.Domain.Persons;
using HCM.Features.Persons.GetAll;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging.Abstractions;

namespace HCM.Tests.CommandHandlers.Persons;

public sealed class GetAllTests
{
    [Fact]
    public async Task GetPersons_ReturnsFilteredByDepartmentForManager()
    {
        await using var context = App.CreateContext();
        var hasher = new PasswordHasher<Person>();
        context.Persons.Add(Person.Create("John","Doe","john@example.com","Dev",100m,"IT",ApplicationRoles.Employee,hasher,"pass"));
        context.Persons.Add(Person.Create("Ann","Lee","ann@example.com","Dev",100m,"Sales",ApplicationRoles.Employee,hasher,"pass"));
        await context.SaveChangesAsync();
        var handler = new GetPersonsQueryHandler(context, NullLogger<GetPersonsQueryHandler>.Instance);

        var result = await handler.Handle(new GetPersonsQuery(1,10,ApplicationRoles.Manager,"IT"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Persons);
    }
}