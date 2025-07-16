using System;
using System.Collections.Generic;

namespace DbManager.Models;

public partial class File
{
    public int Id { get; set; }

    public int? Size { get; set; }

    public string EntityType { get; set; } = null!;

    public int EntityId { get; set; }

    public DateTime UploadedAt { get; set; }
}
