using Application.ModelsDTO;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO;

[PrimaryKey("Id")]
public class ModuleResponseDTO
{
    public int Id { get; set; }
    public string? IdentifierName { get; set; }
    [Key]
    public Guid Identifier { get; set; }
    [Required(ErrorMessage = "Module name is required."), Key]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "AuthorId is required."), Key]
    public string? Description { get; set; }
    public int AuthorId { get; set; }

    public DateTime CreatedAt { get; set; }

    public ICollection<WordResponseDTO> Words { get; set; } = new List<WordResponseDTO>();
}
