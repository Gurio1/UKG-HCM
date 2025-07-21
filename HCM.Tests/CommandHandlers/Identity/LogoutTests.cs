using HCM.Features.Identity.Logout;
using Microsoft.Extensions.Logging.Abstractions;

namespace HCM.Tests.CommandHandlers.Identity;

public sealed class LogoutTests
{
    [Fact]
    public async Task Logout_RemovesRefreshToken()
    {
        await using var context = App.CreateContext();
        var token = Domain.Identity.RefreshTokens.RefreshToken.Create(Guid.NewGuid(), 1);
        context.RefreshTokens.Add(token);
        await context.SaveChangesAsync();

        var handler = new LogoutCommandHandler(context, NullLogger<LogoutCommandHandler>.Instance);
        var result = await handler.Handle(new LogoutCommand(token.Token), CancellationToken.None);

        Assert.True(result.IsFailure);
    }
}