using DomainData.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Application.ModelsDTO
{
    [RequireNumberOrEmail<UserLoginDTO>]
    public class UserLoginDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string? Email { get; set; }
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string? Number { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        [MaxLength(100, ErrorMessage = "Password must be less than or equal to 100 characters.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "ClientId is required.")]
        public string ClientId { get; set; } = null!;
    }
}
