using HCM.Domain.Identity.AccessTokens;
using HCM.Features.Identity;
using HCM.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace HCM.Tests;

public class App
{
    internal static ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new ApplicationDbContext(options);
    }

    internal static TokenIssuer CreateTokenIssuer(ApplicationDbContext context)
    {
        var options = Options.Create(new JwtOptions
        {
            JwtSecret = "super-secret-key-super-secret-key",
            Issuer = "test",
            Audience = "test",
            AccessTokenMinutes = 5,
            RefreshTokenHours = 1
        });
        var generator = new JwtTokenGenerator(options);
        return new TokenIssuer(generator, context, options);
    }
    }
