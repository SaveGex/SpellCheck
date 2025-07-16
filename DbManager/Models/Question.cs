using System;
using System.Collections.Generic;

namespace DbManager.Models;

public partial class Question
{
    public int Id { get; set; }

    public string Expression { get; set; } = null!;

    public string CorrectVariant { get; set; } = null!;
}
