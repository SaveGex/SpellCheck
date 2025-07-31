using Infrastructure.Models;

public class UserDTO
{
    public int? Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Number { get; set; }

    public string? Email { get; set; }

    public DateTime? CreatedAt { get; set; }

    public UserDTO()
    {
    }

    public UserDTO(User user)
    {
        this.Id = user.Id;
        this.Username = user.Username;
        this.Password = user.Password;
        this.Number = user.Number;
        this.Email = user.Email;
        this.CreatedAt = user.CreatedAt;
    }
}
