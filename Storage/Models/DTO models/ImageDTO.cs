namespace LargeFileStorage.Models.DTO_models;

public class ImageDTO
{
    public int Id { get; set; }
    public int ContentTypeId { get; set; }
    public long Size { get; set; }
    public byte[]? Data { get; set; }
    public ImageDTO(int id, int contentTypeId, long size, byte[]? data)
    {
        Id = id;
        ContentTypeId = contentTypeId;
        Size = size;
        Data = data;
    }
    public ImageDTO(Image image)
    {
        Id = image.Id;
        ContentTypeId = image.ContentTypeId;
        Size = image.Size;
        Data = image.Data;
    }
    public ImageDTO() { }
}
