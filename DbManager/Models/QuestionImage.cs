using System;
using System.Collections.Generic;

namespace DbManager.Models;

public partial class QuestionImage
{
    public int Id { get; set; }

    public string? QuestionImage1 { get; set; }

    public string? AnswerImage { get; set; }

    public int QuestionId { get; set; }

    public virtual Question Question { get; set; } = null!;
}
