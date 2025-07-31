using DbManagerApi.JsonPatchSample;
using Infrastructure;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
});

builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddOpenApi();
builder.Services.AddDbContext<SpellTestDbContext>();


var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
