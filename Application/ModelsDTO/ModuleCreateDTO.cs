using Application.ModelsDTO;
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
    [Required(ErrorMessage = "AuthorId is required."), Key]
    public string? Description { get; set; }

    public int AuthorId { get; set; }
    public ICollection<WordCreateDTO>? Words { get; set; } = new List<WordCreateDTO>();

}
