using HCM.Domain.Identity;
using HCM.Domain.Persons;
using HCM.Features.Persons.Create;
using HCM.Shared;
using HCM.Shared.Persons;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace HCM.Tests.CommandHandlers.Persons;

public sealed class CreateTests
{
    [Fact]
    public async Task CreatePerson_Succeeds()
    {
        await using var context = App.CreateContext();
        var creator = new PersonCreator(context, NullLogger<PersonCreator>.Instance);
        var handler = new CreatePersonCommandHandler(creator, new PasswordHasher<Person>(), NullLogger<CreatePersonCommandHandler>.Instance);
        var req = new CreatePersonRequest("John","Doe","john@example.com","Dev",100m,"pass",ApplicationRoles.Employee,"IT");

        var result = await handler.Handle(new CreatePersonCommand(req), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, await context.Persons.CountAsync());
    }
}