using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace DomainData.Models;

[PrimaryKey("Id")]
public partial class Word
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Word cannot be made by itself")]
    public int AuthorId { get; set; }
    [Required(ErrorMessage = "You cannot create a word beyond the module.")]
    public int ModuleId { get; set; }
    [StringLength(maximumLength: 256, ErrorMessage = "Expression cannot be longer than 256 characters.")]
    public string Expression { get; set; } = null!;
    [StringLength(maximumLength: 256, ErrorMessage = "Meaning cannot be longer than 256 characters.")]
    public string Meaning { get; set; } = null!;

    public int? DifficultyId { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    public virtual DifficultyLevel? DifficultyNavigation { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public override bool Equals(object? obj)
    {
        if (obj is not Word word)
        {
            return false;
        }

        return ModuleId == word.ModuleId &&
               string.Equals(Expression, word.Expression, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Meaning, word.Meaning, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id, AuthorId, ModuleId, Expression, Meaning, DifficultyId, CreatedAt);
    }
}
