using System;
using System.Collections.Generic;

namespace DbManager.Models;

public partial class Module
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Question> Questions { get; set; } = new List<Question>();

    public virtual User User { get; set; } = null!;
}
