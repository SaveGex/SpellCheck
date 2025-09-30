using DomainData.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO;

[RequireNumberOrEmail<UserRegisterDTO>]
public class UserUpdateDTO
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = null!;
    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = null!;
    public string? Number { get; set; }
    public string? Email { get; set; }

}
