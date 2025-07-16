using DbManager;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellTestDbContext>();
var app = builder.Build();

RouteGroupBuilder users = app.MapGroup("/users");

users.MapGet("/{entities_name}", GetAllEntities);

app.Run();

static async Task<IResult> GetAllEntities(SpellTestDbContext db, string entities_name)
{
    IEnumerable<Microsoft.EntityFrameworkCore.Metadata.IEntityType> entitiesCollection = db.Model
        .GetEntityTypes()
        .Where(e => e.Name.ToLower() == entities_name.ToLower());
    if (!entitiesCollection.Any())
    {
        return TypedResults.NotFound($"No entities found with the name '{entities_name}'.");
    }
    return TypedResults.Json(
        )
}