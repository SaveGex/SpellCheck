using DbManagerApi.Authentication.Handlers;
using DbManagerApi.JsonPatchSample;
using DbManagerApi.Services;
using DbManagerApi.Services.Converters;
using DbManagerApi.Services.Interfaces;
using Infrastructure;
using Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
})
    .AddNewtonsoftJson();

builder.Services.AddOpenApi();
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
builder.Services.AddDbContext<SpellTestDbContext>();
builder.Services.AddScoped<IEntityOwnershipService, EntityOwnershipService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    //app.UseSwagger();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers().RequireAuthorization(policy =>
{
    policy.AuthenticationSchemes.Add("BasicAuthentication");
    policy.RequireAuthenticatedUser();
});

app.Run();
