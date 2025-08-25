using Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models.ModelsDTO
{

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

    [RequireNumberOrEmail]
    public class  UserCreateDTO
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

    [RequireNumberOrEmail]
    public class UserUpdateDTO
    {
        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = null!;
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = null!;
        public string? Number { get; set; }
        public string? Email { get; set; }
    }
}
