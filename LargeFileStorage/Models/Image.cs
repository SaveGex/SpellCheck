using Microsoft.EntityFrameworkCore;

namespace LargeFileStorage.Models;

public class Image
{
    public int Id { get; set; }
    public int ContentTypeId { get; set; }
    public ContentTypes ContentType { get; set; }
    public long Size { get; set; }
    public byte[]? Data { get; set; }
}
