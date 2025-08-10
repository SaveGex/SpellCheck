using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Module
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
}
