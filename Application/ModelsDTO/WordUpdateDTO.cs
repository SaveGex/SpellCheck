using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO;

public class WordUpdateDTO
{

    [Required(ErrorMessage = "You cannot create a word beyond the module.")]
    public int ModuleId { get; set; }
    [StringLength(maximumLength: 256, ErrorMessage = "Expression cannot be longer than 256 characters.")]
    public string Expression { get; set; } = null!;
    [StringLength(maximumLength: 256, ErrorMessage = "Meaning cannot be longer than 256 characters.")]
    public string Meaning { get; set; } = null!;
    public int? DifficultyId { get; set; }
}
