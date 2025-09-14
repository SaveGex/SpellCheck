using DomainData.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO;

[RequireNumberOrEmail]
public class UserCreateDTO
{
    [StringLength(maximumLength: 32, ErrorMessage = "Username cannot be longer than 32 characters.")]
    public string Username { get; set; } = null!;
    [StringLength(maximumLength: 256, ErrorMessage = "Password cannot be longer than 256 characters.")]
    public string Password { get; set; } = null!;
    [StringLength(maximumLength: 25, ErrorMessage = "Number cannot be longer than 25 characters.")]
    public string? Number { get; set; }
    [StringLength(maximumLength: 254, ErrorMessage = "Email cannot be longer than 254 characters.")]
    public string? Email { get; set; }
}
