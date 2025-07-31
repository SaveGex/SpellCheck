using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Word
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Expression { get; set; } = null!;

    public string Meaning { get; set; } = null!;

    public int? Difficulty { get; set; }
    [BindNever]
    public virtual DifficultyLevel? DifficultyNavigation { get; set; }
    [BindNever]
    public virtual ICollection<LearnWord> LearnWords { get; set; } = new List<LearnWord>();
    [BindNever]
    public virtual User User { get; set; } = null!;
}
