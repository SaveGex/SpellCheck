using Infrastructure.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Models;

[RequireNumberOrEmail]
public partial class User 
{
    public int Id { get; set; }
    [StringLength(maximumLength: 32, ErrorMessage = "Username cannot be longer than 32 characters.")]
    public string Username { get; set; } = null!;
    [StringLength(maximumLength: 256, ErrorMessage = "Password cannot be longer than 256 characters.")]
    public string Password { get; set; } = null!;
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
}
