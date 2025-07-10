namespace LargeFileStorage.Models.DTO_models;

public class ContentTypesDTO
{
    public int Id { get; set; }
    public string ContentType { get; set; } = null!;
    public ContentTypesDTO(int id, string contentType)
    {
        Id = id;
        ContentType = contentType;
    }
    public ContentTypesDTO(ContentTypes contentType)
    {
        Id = contentType.Id;
        ContentType = contentType.ContentType;
    }
    public ContentTypesDTO() { }
}
