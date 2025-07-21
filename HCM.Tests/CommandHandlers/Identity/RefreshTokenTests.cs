using HCM.Domain.Identity;
using HCM.Domain.Persons;
using HCM.Features.Identity.Refresh;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace HCM.Tests.CommandHandlers.Identity;

public sealed class RefreshTokenTests
{
    [Fact]
    public async Task RefreshToken_ReturnsNewTokens()
    {
        await using var context = App.CreateContext();
        var hasher = new PasswordHasher<Person>();
        var person = Person.Create("John","Doe","john@example.com","Dev",100m,"IT",ApplicationRoles.Employee,hasher,"pass");
        context.Persons.Add(person);
        var issuer = App.CreateTokenIssuer(context);
        var refresh = Domain.Identity.RefreshTokens.RefreshToken.Create(person.Id, 1);
        context.RefreshTokens.Add(refresh);
        await context.SaveChangesAsync();

        var handler = new RefreshTokenCommandHandler(issuer, context, NullLogger<RefreshTokenCommandHandler>.Instance);
        var result = await handler.Handle(new RefreshTokenCommand(refresh.Token), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, await context.RefreshTokens.CountAsync());
    }
    
    [Fact]
    public async Task RefreshToken_ReturnsInvalid_WhenTokenMissing()
    {
        await using var context = App.CreateContext();
        var issuer = App.CreateTokenIssuer(context);
        var handler = new RefreshTokenCommandHandler(issuer, context, NullLogger<RefreshTokenCommandHandler>.Instance);

        var result = await handler.Handle(new RefreshTokenCommand("unknown"), CancellationToken.None);

        Assert.True(result.IsFailure);
    }
}