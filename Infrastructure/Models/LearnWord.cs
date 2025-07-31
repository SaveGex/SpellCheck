using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class LearnWord
{
    public int Id { get; set; }

    public int WordId { get; set; }

    public int LearningProgress { get; set; }

    public int UserId { get; set; }
    [BindNever]
    public virtual User User { get; set; } = null!;
    [BindNever]
    public virtual Word Word { get; set; } = null!;
}
