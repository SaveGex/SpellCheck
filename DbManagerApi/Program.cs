using Application.Interfaces;
using Application.Profiles;
using Application.Profiles.Resolvers;
using Application.Services;
using DbManagerApi.Authentication.Handlers;
using DbManagerApi.JsonPatchSample;
using DbManagerApi.Services;
using DomainData.Roles;
using Infrastructure.DI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Configuration;
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

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
})
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null) // remained for debugging purposes
    .AddJwtBearer("Bearer", options =>
    {
        var JwtKeySecretWord = builder.Configuration["JWT:KeyWord"];
        if (JwtKeySecretWord is null)
            throw new ConfigurationErrorsException("JWT KeyWord is not configured");

        var JwtKeySecretValue = builder.Configuration.GetValue<string>(JwtKeySecretWord);
        if (JwtKeySecretValue is null)
            throw new ConfigurationErrorsException("JWT Key is not configured");

        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    JwtKeySecretValue
            ))
        };
    });
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

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
