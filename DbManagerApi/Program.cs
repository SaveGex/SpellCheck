using Application.Interfaces;
using Application.Profiles;
using Application.Profiles.Resolvers;
using Application.Services;
using DbManagerApi.Authentication.Handlers;
using DbManagerApi.JsonPatchSample;
using DbManagerApi.Services;
using DomainData.Roles;
using Infrastructure.DI;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

const string LuckyLicenseKeyWord = "Lucky Penny License Key";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
})
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();


builder.Services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null) // remained for debugging purposes
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration.GetJWTSigningSecretValue()
            )),
            ValidateAudience = false // do it later, manually
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = async context =>
            {
                var db = context.HttpContext.RequestServices
                    .GetRequiredService<Infrastructure.DB.SpellTestDbContext>();

                var audiences = await db.Clients
                    .Select(c => c.URL)
                    .ToListAsync();

                // fetch all 'aud' claims
                var jwtAudiences = context.Principal?
                    .FindAll("aud")
                    .Select(c => c.Value)
                    .ToList();

                if (jwtAudiences == null || !jwtAudiences.Any())
                {
                    context.Fail("Token has no audience claim.");
                    return;
                }

                // check if at least one matches
                if (!jwtAudiences.Any(aud => audiences.Contains(aud)))
                {
                    context.Fail("Token audience not allowed.");
                }
            }
        };
    });
builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
    auth.AddPolicy("BasicAuthentication", policy =>
    {
        policy.AddAuthenticationSchemes("BasicAuthentication");
        policy.RequireAuthenticatedUser();
    });
});

//---add repositories to the DI container ---
builder.Services.AddInfrastructure();

//---add services---
builder.Services.AddTransient<IEntityOwnershipService, EntityOwnershipService>();
builder.Services.AddTransient<IModuleService, ModuleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IWordService, WordService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<IDifficultyLevelService, DifficultyLevelService>();
builder.Services.AddTransient<IFriendsService, FriendsService>();
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<IClientService, ClientService>();

//---add auto mapper ---
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
    cfg.LicenseKey = builder.Configuration.GetValue<string>(LuckyLicenseKeyWord);
});
builder.Services.AddTransient<PasswordHashResolver>();

builder.Services.AddPagination();
builder.Services.AddMemoryCache();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi("/openapi/DbManagerApi.json");
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/openapi/DbManagerApi.json";
    });

}

app.UseHttpsRedirection();

app.MapControllers().RequireAuthorization(policy =>
{
    policy.AddAuthenticationSchemes("BasicAuthentication", "Bearer");

    policy.RequireAuthenticatedUser();
    policy.RequireRole(nameof(RoleNames.User), nameof(RoleNames.Manager), nameof(RoleNames.Admin));
});

app.Run();
