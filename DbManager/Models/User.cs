using System;
using System.Collections.Generic;

namespace DbManager.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Number { get; set; }

    public string? Email { get; set; }

    public virtual ICollection<Module> Modules { get; set; } = new List<Module>();
}
