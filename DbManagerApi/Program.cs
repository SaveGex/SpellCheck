using Application.Interfaces;
using Application.Profiles;
using Application.Services;
using AutoMapper;
using DbManagerApi.Authentication.Handlers;
using DbManagerApi.JsonPatchSample;
using DbManagerApi.Services;
using Infrastructure.DI;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;

const string LuckyLicenseKeyWord = "Lucky Penny License Key";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
})
    .AddNewtonsoftJson();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();


builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//---add services---
builder.Services.AddTransient<IEntityOwnershipService, EntityOwnershipService>();
builder.Services.AddTransient<IModuleService, ModuleService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IWordService, WordService>();

//---add repositories to the DI container ---
builder.Services.AddInfrastructure();

//---add auto mapper ---
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MappingProfile>();
    cfg.LicenseKey = builder.Configuration.GetValue<string>(LuckyLicenseKeyWord);
});

builder.Services.AddPagination();

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
    policy.AuthenticationSchemes.Add("BasicAuthentication");
    policy.RequireAuthenticatedUser();
});

app.Run();
