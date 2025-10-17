using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.ModelsDTO;

public class ModuleCreateDTO
{
    public string? IdentifierName { get; set; }
    [Key, JsonIgnore]
    public Guid? Identifier { get; set; }
    [Required(ErrorMessage = "Module name is required."), Key]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }
    [Required(ErrorMessage = "AuthorId is required."), Key]
    public int AuthorId { get; set; }
    public ICollection<WordCreateDTO>? Words { get; set; } = new List<WordCreateDTO>();

}
