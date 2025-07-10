namespace LargeFileStorage.Models;

public class ContentTypes
{
    public int Id { get; set; }
    public string ContentType { get; set; }
    public ICollection<Image> Images { get; set; }
}
