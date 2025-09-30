using DomainData.Attributes;
using DomainData.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace DomainData.Models;

[RequireNumberOrEmail<User>]
public partial class User : ISoftDeletableTimeStamp
{
    public int Id { get; set; }
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
    [Required]
    public string PasswordHash { get; set; } = null!;
    [StringLength(maximumLength: 25, ErrorMessage = "Number cannot be longer than 25 characters.")]
    public string? Number { get; set; }
    [StringLength(maximumLength: 254, ErrorMessage = "Email cannot be longer than 254 characters.")]
    public string? Email { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
    [DataType(DataType.DateTime)]
    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Friend> FriendFromIndividuals { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendToIndividuals { get; set; } = new List<Friend>();

    public virtual ICollection<Module> CreatedModules { get; set; } = new List<Module>();

    public virtual ICollection<Module> UserModules { get; set; } = new List<Module>();

    public virtual ICollection<Word> Words { get; set; } = new List<Word>();

    /// <summary>
    /// Naviagation property but using in code for define user roles.
    /// </summary>
    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
