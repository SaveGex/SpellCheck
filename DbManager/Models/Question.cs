using System;
using System.Collections.Generic;

namespace DbManager.Models;

public partial class Question
{
    public int Id { get; set; }

    public int ModuleId { get; set; }

    public string Question1 { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public virtual Module Module { get; set; } = null!;

    public virtual ICollection<QuestionImage> QuestionImages { get; set; } = new List<QuestionImage>();
}
