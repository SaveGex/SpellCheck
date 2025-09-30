using System.ComponentModel.DataAnnotations;

namespace DomainData.Models;

public class Role
{
    public int Id { get; set; }
    [StringLength(maximumLength: 128, ErrorMessage = "Role name cannot be longer than 128 characters.")]
    public string Name { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
