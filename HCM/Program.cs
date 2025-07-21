using System.Security.Claims;
using FastEndpoints;
using FastEndpoints.Security;
using FastEndpoints.Swagger;
using HCM.Domain.Identity.AccessTokens;
using HCM.Domain.Identity.Authentication.RequirementHandlers;
using HCM.Domain.Identity.Authentication.Requirements;
using HCM.Domain.Persons;
using HCM.Features.Identity;
using HCM.Infrastructure;
using HCM.Shared.Persons;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IAuthorizationHandler, ViewPersonRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, EditPersonRequirementHandler>();

builder.Services
    .AddAuthenticationJwtBearer(o =>
    {
        o.SigningKey = builder.Configuration["Auth:JWT:JwtSecret"];
    });

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5221);
});

builder.Services
    .AddAuthorizationBuilder()
    .AddPolicy("CanViewPerson", policy =>
    {
        policy.Requirements.Add(new ViewPersonRequirement());
    })
    .AddPolicy("CanEditPerson", policy =>
        {
            policy.Requirements.Add(new EditPersonRequirement());
        });

builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.AutoTagPathSegmentIndex = 2;
        o.ShortSchemaNames = true;
        
    });

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Auth:Jwt"));

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddScoped<JwtTokenGenerator>();
builder.Services.AddScoped<TokenIssuer>();
builder.Services.AddScoped<PersonCreator>();
builder.Services.AddScoped<PersonGetter>();
builder.Services.AddScoped<IPasswordHasher<Person>, PasswordHasher<Person>>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder =>
        {
            policyBuilder.WithOrigins("https://localhost:4200", "http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<Person>>();
    
    await SeedDb.SeedAsync(context, passwordHasher);
}

app.MapMetrics("/metrics");
app.UseHttpMetrics();

app.UseCors("AllowSpecificOrigin");


app.UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints(c => c.Security.RoleClaimType = ClaimTypes.Role)
    .UseSwaggerGen();


app.Run();
