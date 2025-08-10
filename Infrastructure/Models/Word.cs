using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

public partial class Word
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ModuleId { get; set; }
    [StringLength(maximumLength: 256, ErrorMessage = "Expression cannot be longer than 256 characters.")]
    public string Expression { get; set; } = null!;
    [StringLength(maximumLength: 256, ErrorMessage = "Meaning cannot be longer than 256 characters.")]
    public string Meaning { get; set; } = null!;

    public int? Difficulty { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual DifficultyLevel? DifficultyNavigation { get; set; }

    public virtual Module Module { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
