using DbManager;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SpellTestDbContext>();
var app = builder.Build();
RouteGroupBuilder users = app.MapGroup("/users");

app.MapGet("/{entities_name}", GetAllEntities);
app.MapGet("/{entity_name}/{id:int}", GetEntityById);
app.MapPost("/{entity_name}", AddEntity);
app.Run();

static Task<IResult> GetAllEntities(SpellTestDbContext db, string entities_name)
{
    PropertyInfo? prop;
    IEnumerable<IEntityType?> entityTypes = db.Model
        .GetEntityTypes();
    IEntityType? entityType;

    entityType = entityTypes.FirstOrDefault(e =>
    (
        e is not null ?
        (e.ClrType.Name.ToLower() + 's' == entities_name.ToLower())
        : false)
    );


    if (entityType is not null)
    {
        prop = typeof(SpellTestDbContext)
            .GetProperty(entityType.ClrType.Name + 's', BindingFlags.Public | BindingFlags.Instance);
    }
    else
    {
        return Task.FromResult<IResult>(TypedResults.NotFound($"Entity type '{entities_name}' not found."));
    }

    if (prop is not null)
    {
        var entities = prop.GetValue(db) as IQueryable<object>;
        return Task.FromResult<IResult>(TypedResults.Json(entities));
    }
    else
    {
        return Task.FromResult<IResult>(TypedResults.NotFound($"Property '{entities_name} + s' not found in DbContext."));
    }
}


static Task<IResult> GetEntityById(SpellTestDbContext db, string entity_name, int id)
{
    PropertyInfo? prop;
    Microsoft.EntityFrameworkCore.Metadata.IEntityType? entityType = db.Model
        .GetEntityTypes()
        .FirstOrDefault(e => e.ClrType.Name.ToLower() + 's' == entity_name.ToLower());
    if (entityType is not null)
    {
        prop = typeof(SpellTestDbContext)
            .GetProperty(entityType.ClrType.Name + 's', BindingFlags.Public | BindingFlags.Instance);
    }
    else
    {
        return Task.FromResult<IResult>(TypedResults.NotFound($"Entity type '{entity_name}' not found."));
    }

    if (prop is not null)
    {
        IQueryable<object>? entities = prop.GetValue(db) as IQueryable<object>;
        if (entities is null)
        {
            return Task.FromResult<IResult>(TypedResults.NotFound($"No entities found for '{entity_name}'."));
        }

        object? entity = null;
        foreach (object obj in entities)
        {
            if (obj.GetType()?.GetProperty("Id")?.GetValue(obj) is int value && value == id)
            {
                entity = obj;
            }
        }
        return Task.FromResult<IResult>(entity is not null ? TypedResults.Json(entity) : TypedResults.NoContent());
    }
    else
    {
        return Task.FromResult<IResult>(TypedResults.NotFound($"Property '{entity_name} + s' not found in DbContext."));
    }
}


static async Task<IResult> AddEntity(SpellTestDbContext db, string entity_name, HttpContext context)
{
    IEntityType? entityType = db.Model
        .GetEntityTypes()
        .FirstOrDefault(e => e.ClrType.Name.ToLower() + 's' == entity_name.ToLower());
    if (entityType is null)
    {
        return TypedResults.NotFound($"Entity type '{entity_name}' not found.");
    }
    object? entity = Activator.CreateInstance(entityType.ClrType);
    if (entity is null)
    {
        return TypedResults.BadRequest($"Could not create instance of '{entity_name}'.");
    }
    // Assuming the request body contains the JSON representation of the entity
    context.Request.EnableBuffering();
    using var reader = new StreamReader(context.Request.Body);
    string body = await reader.ReadToEndAsync();
    context.Request.Body.Position = 0;
    // Deserialize the JSON into the entity object
    try
    {
        entity = System.Text.Json.JsonSerializer.Deserialize(body, entityType.ClrType);
        if (entity is not null)
        {

            if (await EntityRecordAlreadyExists(entityType, entity, db))
            {
                return TypedResults.BadRequest("Entity record already exists.");
            }
            db.Add(entity);
            db.SaveChanges();
            context.Response.Headers.Append("Entity-Id", $"{entity.GetType().GetProperty("Id")?.GetValue(entity)}");
            return TypedResults.Created($"/{entity_name}/{entity.GetType().GetProperty("Id")?.GetValue(entity)}", entity);
        }
        else
        {
            return TypedResults.StatusCode(500); // 500 Internal Server Error
        }
    }
    catch (Exception ex)
    {
        return TypedResults.BadRequest($"Error deserializing entity: {ex.Message}");
    }
}

///<summary>
/// Checks if an entity record already exists in the database based on unique properties.
///</summary>
///<returns>A task of the result</returns>
///<returns>True if the entity already exists, otherwise false.</returns>
static Task<bool> EntityRecordAlreadyExists(IEntityType entityType, object objectData, SpellTestDbContext db)
{
    IEnumerable<string> uniquePropertyNames = db.Model
        .GetEntityTypes()
        .SelectMany(entityType => entityType.GetIndexes()
        .Where(index => index.IsUnique)
            .SelectMany(index => index.Properties
            .Select(prop => prop.Name)))
    .Distinct()
    .ToList();

    IQueryable<object?>? dbSet = typeof(SpellTestDbContext)
        .GetProperty(entityType.ClrType.Name + 's')
        ?.GetValue(db) as IQueryable<object?>;

    IEnumerable<IProperty> efUniqueRuntimeProperties = entityType.GetProperties()
        .Where(p => uniquePropertyNames.Contains(p.Name));

    if (dbSet is null || dbSet.IsNullOrEmpty() || efUniqueRuntimeProperties.IsNullOrEmpty())
    {
        return Task.FromResult(false);
    }

    int numberOfUniqueObjectProperties = 0;
    foreach (IProperty efProperty in efUniqueRuntimeProperties)
    {
        foreach (PropertyInfo objProperty in objectData.GetType().GetProperties())
        {
            if (objProperty.Name == efProperty.Name && objProperty.GetValue(objectData) is not null)
            {
                numberOfUniqueObjectProperties++;
            }
        }
    }
    if (numberOfUniqueObjectProperties == 0)
    {
        return Task.FromResult(false); // Not enough unique properties to consider it a duplicate
    }

    foreach (object? obj in dbSet)
    {
        foreach (RuntimeProperty efProperty in efUniqueRuntimeProperties)
        {
            var property = objectData.GetType().GetProperty(efProperty.Name);
            if (property == null) continue;
            object? value = property.GetValue(objectData);
            object? existingValue = property.GetValue(obj);
            if (value is not null && existingValue is not null && value.Equals(existingValue))
            {
                return Task.FromResult(true);
            }
        }
        ;


    }

    return Task.FromResult(false);
}

