using System;
using System.Collections.Generic;

namespace DbManagerApi.Models;

public partial class WordsToLearn
{
    public int Id { get; set; }

    public int LearningProgress { get; set; }

    public string Expression { get; set; } = null!;

    public string Meaning { get; set; } = null!;

    public int? Difficulty { get; set; }

    public int UserId { get; set; }

    public virtual DifficultyLevel? DifficultyNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
