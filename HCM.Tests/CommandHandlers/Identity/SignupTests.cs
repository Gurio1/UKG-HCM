using HCM.Domain.Persons;
using HCM.Features.Identity.Signup;
using HCM.Shared;
using HCM.Shared.Persons;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace HCM.Tests.CommandHandlers.Identity;

public sealed class SignupTests
{
    [Fact]
    public async Task Signup_CreatesPersonAndReturnsTokens()
    {
        await using var context = App.CreateContext();
        var creator = new PersonCreator(context,NullLogger<PersonCreator>.Instance);
        var hasher = new PasswordHasher<Person>();
        var issuer = App.CreateTokenIssuer(context);
        var handler = new SignupCommandHandler(creator, issuer, hasher, NullLogger<SignupCommandHandler>.Instance);
        var request = new SignupRequest("John","Doe","john@example.com","Dev",100m,"IT","pass","pass");

        var result = await handler.Handle(new SignupCommand(request), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value.AccessToken);
        Assert.Equal(1, await context.Persons.CountAsync());
        Assert.Equal(1, await context.RefreshTokens.CountAsync());
    }
}