using LargeFileStorage;
using LargeFileStorage.Models;
using LargeFileStorage.Models.DTO_models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<Db>(opt => opt.UseSqlite("Data Source=largefiles.db"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

var images = app.MapGroup("/images");

images.MapGet("", GetAllImages);
images.MapGet("/{id}", GetImageBytId);
images.MapPost("", CreateRecord);
images.MapPut("/{id}", ChangeImage);
images.MapDelete("/{id}", DeleteImageById);
var contentTypes = app.MapGroup("/contenttypes");

contentTypes.MapGet("", GetAllContentTypes);
contentTypes.MapGet("/{id}", GetContentTypeById);
contentTypes.MapPost("", CreateContentType);
contentTypes.MapDelete("{id}", DeleteContentTypeById);
app.Run();

static async Task<IResult> GetAllImages(Db db)
{
    return TypedResults.Json(await db.Images.Select(i => new ImageDTO(i)).ToArrayAsync());
}
static async Task<IResult> GetImageBytId(int? id, Db db, HttpContext context)
{
    if(id is null)
    {
        return TypedResults.BadRequest("id must have a value");
    }

    Image? entity = await db.Images.FindAsync(id);

    if(entity is null)
    {
        return TypedResults.NotFound();
    }

    context.Response.Headers.Append("Entity-Id", $"{entity.Id}");

    return TypedResults.Json(new ImageDTO(entity));


}
static async Task<IResult> CreateRecord(Image? image, Db db, HttpContext context)
{
    if(image is null)
    {
        return TypedResults.BadRequest("Image data is required.");
    }
    db.Images.Add(image);
    await db.SaveChangesAsync();

    context.Response.Headers.Append("Entity-Id", $"{image.Id}");

    return TypedResults.Created($"/images/{image.Id}");
}
static async Task<IResult> ChangeImage(int? id, Image? image, Db db, HttpContext context)
{
    if(id is null)
    {
        return TypedResults.BadRequest("Id is required.");
    }
    if(image is null)
    {
        return TypedResults.BadRequest("Image data is required.");
    }

    Image? entity = await db.Images.FindAsync(id);

    if(entity is null)
    {
        return TypedResults.NotFound();
    }

    entity.ContentTypeId = image.ContentTypeId;
    entity.Size = image.Size;
    entity.Data = image.Data;
    db.Images.Update(entity);
    await db.SaveChangesAsync();

    context.Response.Headers.Append("Entity-Id", $"{entity.Id}");

    return Results.Ok("Image was succesfully changed");
}
static async Task<IResult> DeleteImageById(int? id, Db db, HttpContext context)
{
    if(id is null)
    {
        return TypedResults.BadRequest("Id must be not null");
    }

    Image? entity = await db.Images.FindAsync(id);
    if(entity is null)
    {
        return TypedResults.NotFound();
    }

    db.Images.Remove(entity);
    await db.SaveChangesAsync();

    context.Response.Headers.Append("Entity-Id", $"{entity.Id}");

    return TypedResults.Ok("Succesfully deleted image");
}


static async Task<IResult> GetAllContentTypes(Db db)
{
    return TypedResults.Json(await db.ContentTypes.Select(ct => new ContentTypesDTO(ct)).ToArrayAsync());
}
static async Task<IResult> GetContentTypeById(int? id, Db db, HttpContext context)
{
    ContentTypes? entity = await db.ContentTypes.FindAsync(id);
    if (entity is null)
    {
        return TypedResults.NotFound("Content is not found");
    }

    context.Response.Headers.Append("Entity-Id", $"{entity.Id}");

    return TypedResults.Json(new ContentTypesDTO(entity));
}
static async Task<IResult> CreateContentType(ContentTypes? contentType, Db db, HttpContext context)
{
    if(contentType is null)
    {
        return TypedResults.BadRequest("Content type data is required.");
    }
    db.ContentTypes.Add(contentType);
    await db.SaveChangesAsync();

    context.Response.Headers.Append("Entity-Id", $"{contentType.Id}");

    return TypedResults.Created($"/contenttypes/{contentType.Id}");
}
static async Task<IResult> DeleteContentTypeById(int? id,  Db db, HttpContext context)
{
    if(id is null)
    {
        return TypedResults.BadRequest("Id must have a value");
    }

    ContentTypes? entity = await db.ContentTypes.FindAsync(id);

    if (entity is null)
    {
        return TypedResults.NotFound();
    }

    db.ContentTypes.Remove(entity);
    await db.SaveChangesAsync();
    context.Response.Headers.Append("Entity-Id", $"{entity.Id}");

    return TypedResults.Ok("Succesfully deleted content type");
}