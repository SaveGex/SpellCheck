using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;

namespace Infrastructure.Models;

public partial class Friend
{
    public int Id { get; set; }

    public int FromIndividualId { get; set; }

    public int ToIndividualId { get; set; }
    [BindNever]
    public virtual User FromIndividual { get; set; } = null!;
    [BindNever]
    public virtual User ToIndividual { get; set; } = null!;
}
