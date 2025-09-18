
using System.ComponentModel;

namespace DomainData.Models;

public partial class DifficultyLevel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [Description("Has DB constraint which is checks a value is in the range between 1 and 6 (inclusive) or not.")]
    public int Difficulty { get; set; }

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
}
