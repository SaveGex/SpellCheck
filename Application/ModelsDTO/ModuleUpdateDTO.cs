using Application.ModelsDTO;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Application.ModelsDTO;



public class ModuleUpdateDTO
{
    public string? IdentifierName { get; set; }
    [Key, JsonIgnore]
    public Guid? Identifier { get; set; }
    [Required(ErrorMessage = "Module name is required."), Key]
    public string Name { get; set; } = null!;
    public string? Description { get; set; }

    public ICollection<WordUpdateDTO>? Words { get; set; } = new List<WordUpdateDTO>();
}