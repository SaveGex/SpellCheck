using DomainData.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO;

[RequireNumberOrEmail<UserRegisterDTO>]
public class UserRegisterDTO
{
    [StringLength(maximumLength: 512, ErrorMessage = "FirstName cannot be longer than 512 characters.")]
    public string? FirstName { get; set; }
    [StringLength(maximumLength: 512, ErrorMessage = "LastName cannot be longer than 512 characters.")]
    public string? LastName { get; set; }
    [Required]
    [StringLength(maximumLength: 32, ErrorMessage = "Username cannot be longer than 32 characters.")]
    public string Username
    {
        get => field;
        set
        {
            FirstName = FirstName ?? value;
            field = value ?? field;
        }
    } = null!;
    [Required]
    [StringLength(maximumLength: 256, MinimumLength = 6, ErrorMessage = "Password cannot be longer than 256 and less than 6 characters.")]
    public string Password { get; set; } = null!;
    [Phone]
    [StringLength(maximumLength: 25, ErrorMessage = "Number cannot be longer than 25 characters.")]
    public string? Number { get; set; }
    [EmailAddress]
    [StringLength(maximumLength: 254, ErrorMessage = "Email cannot be longer than 254 characters.")]
    public string? Email { get; set; }
}
