using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace Infrastructure.Models;

[PrimaryKey("Id")]
public partial class Module
{
    public int Id { get; set; }
    [Key]
    public string? IdentifierName { get; set; }

    [Required(ErrorMessage = "Identifier is required"), Key]
    public Guid Identifier { get; set; }

    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Module cannot exists without author")]
    public int AuthorId { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    public virtual User Author { get; set; } = null!;

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public override bool Equals(object? obj)
    {
        if(obj is not Module module)
        {
            return false;
        }

        if(module.GetHashCode() == this.GetHashCode())
        {
            return true;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Identifier);
    }
}
