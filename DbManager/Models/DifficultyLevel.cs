using System;
using System.Collections.Generic;

namespace DbManager.Models;

public partial class DifficultyLevel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Difficulty { get; set; }

    public virtual ICollection<WordsToLearn> WordsToLearns { get; set; } = new List<WordsToLearn>();
}
