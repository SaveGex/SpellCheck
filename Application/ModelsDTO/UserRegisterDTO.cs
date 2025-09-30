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
    private string _username = null!;
    [StringLength(maximumLength: 32, ErrorMessage = "Username cannot be longer than 32 characters.")]
    public string Username
    {
        get => _username;
        set
        {
            if (FirstName is null && value is not null)
            {
                FirstName = value;
            }
            if (value is not null)
            {
                _username = value;
            }
        }
    }
    [StringLength(maximumLength: 256, MinimumLength = 6, ErrorMessage = "Password cannot be longer than 256 and less than 6 characters.")]
    public string Password { get; set; } = null!;
    [StringLength(maximumLength: 25, ErrorMessage = "Number cannot be longer than 25 characters.")]
    public string? Number { get; set; }
    [EmailAddress]
    [StringLength(maximumLength: 254, ErrorMessage = "Email cannot be longer than 254 characters.")]
    public string? Email { get; set; }
}
