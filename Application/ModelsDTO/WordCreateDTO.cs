using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO;

public class WordCreateDTO
{
    [Required(ErrorMessage = "UserId is required.")]
    public int AuthorId { get; set; }
    [Required(ErrorMessage = "You cannot create a word beyond the module.")]
    public int ModuleId { get; set; }
    [StringLength(maximumLength: 256, ErrorMessage = "Expression cannot be longer than 256 characters.")]
    public string Expression { get; set; } = null!;
    [StringLength(maximumLength: 256, ErrorMessage = "Meaning cannot be longer than 256 characters.")]
    public string Meaning { get; set; } = null!;
    public int? DifficultyId { get; set; }
}
