
namespace Infrastructure.Models;

public partial class DifficultyLevel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? Difficulty { get; set; }

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
}
