using DbManagerApi.Authentication.Handlers;
using DbManagerApi.JsonPatchSample;
using DbManagerApi.Services;
using DbManagerApi.Services.Abstracts;
using DbManagerApi.Services.Interfaces;
using DbManagerApi.Services.ModuleServices;
using Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

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

builder.Services.AddDbContext<SpellTestDbContext>();
builder.Services.AddScoped<IEntityOwnershipService, EntityOwnershipService>();
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
