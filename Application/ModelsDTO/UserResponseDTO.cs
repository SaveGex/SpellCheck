
namespace Application.ModelsDTO;

public class UserResponseDTO
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string? Number { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<RoleResponseDTO> Roles { get; set; } = new List<RoleResponseDTO>();
}
