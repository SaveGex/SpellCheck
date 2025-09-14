
namespace Application.ModelsDTO;

public class UserResponseDTO
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Number { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    //public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
