using System;
using System.Collections.Generic;

namespace DbManagerApi.Models;

public partial class Friend
{
    public int Id { get; set; }

    public int FromIndividualId { get; set; }

    public int ToIndividualId { get; set; }

    public virtual User FromIndividual { get; set; } = null!;

    public virtual User ToIndividual { get; set; } = null!;
}
