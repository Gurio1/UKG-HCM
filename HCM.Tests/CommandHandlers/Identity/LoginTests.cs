using HCM.Domain.Identity;
using HCM.Domain.Persons;
using HCM.Features.Identity.Login;
using Microsoft.AspNetCore.Identity;

namespace HCM.Tests.CommandHandlers.Identity;

public sealed class LoginTests
{
    [Fact]
    public async Task Login_ReturnsInvalid_WhenEmailNotFound()
    {
        await using var context = App.CreateContext();
        var handler = new LoginCommandHandler(context, new PasswordHasher<Person>());

        var result = await handler.Handle(new LoginCommand("a@b.c", "pass"), CancellationToken.None);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Login_ReturnsInvalid_WhenPasswordWrong()
    {
        await using var context = App.CreateContext();
        var hasher = new PasswordHasher<Person>();
        var person = Person.Create("John","Doe","john@example.com","Dev",100m,"IT",ApplicationRoles.Employee,hasher,"pass");
        context.Persons.Add(person);
        await context.SaveChangesAsync();

        var handler = new LoginCommandHandler(context, hasher);
        var result = await handler.Handle(new LoginCommand("john@example.com", "wrong"), CancellationToken.None);

        Assert.True(result.IsFailure);
    }

    [Fact]
    public async Task Login_ReturnsSuccess_WithValidCredentials()
    {
        await using var context = App.CreateContext();
        var hasher = new PasswordHasher<Person>();
        var person = Person.Create("John","Doe","john@example.com","Dev",100m,"IT",ApplicationRoles.Employee,hasher,"pass");
        context.Persons.Add(person);
        await context.SaveChangesAsync();

        var handler = new LoginCommandHandler(context, hasher);
        var result = await handler.Handle(new LoginCommand("john@example.com", "pass"), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(person.Id, result.Value.Id);
    }
}