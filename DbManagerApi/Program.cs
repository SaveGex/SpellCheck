using Application.Interfaces;
using Application.Profiles;
using Application.Profiles.Resolvers;
using Application.Services;
using DbManagerApi.Extentions;
using DbManagerApi.JsonPatchSample;
using DbManagerApi.Services;
using DomainData.Roles;
using Infrastructure.DI;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

const string LuckyLicenseKeyWord = "Lucky Penny License Key";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
})
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

builder.Services.AddLogging(logBuilder =>
{
    logBuilder.AddConfiguration(builder.Configuration);
    
    logBuilder.AddFileLogging(builder.Configuration);
});

builder.Services.AddAuthentication()
    //.AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null) // remained for debugging purposes
    .AddJwtBearer();

builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, JwtBearerOptionsConfigurator>();

builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", policy =>
    {
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddHealthChecks();

//---add repositories to the DI container ---
builder.AddInfrastructure();

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

var app = builder.Build();

app.ExecuteMigrations();

await app.RegisterAdminUserAsync();
await app.RegisterTestClientAsync();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

//if (app.Environment.IsDevelopment())
if(true)
{
    app.MapOpenApi("/openapi/DbManagerApi.json");
    app.MapScalarApiReference(options =>
    {
        options.OpenApiRoutePattern = "/openapi/DbManagerApi.json";
    });

}

app.UseHttpsRedirection();

app.MapHealthChecks("/health");

app.MapControllers().RequireAuthorization(policy =>
{
    //policy.AddAuthenticationSchemes("BasicAuthentication", "Bearer");
    policy.AddAuthenticationSchemes("Bearer");

    policy.RequireAuthenticatedUser();
    policy.RequireRole(nameof(RoleNames.User), nameof(RoleNames.Manager), nameof(RoleNames.Admin));
});

app.Run();
