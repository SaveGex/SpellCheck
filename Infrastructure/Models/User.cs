using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Infrastructure.Models;

public partial class User
{
    [JsonIgnore]
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Number { get; set; }

    public string? Email { get; set; }
    [JsonIgnore]
    public DateTime CreatedAt { get; set; }
    [JsonIgnore]
    public virtual ICollection<Friend> FriendFromIndividuals { get; set; } = new List<Friend>();
    [JsonIgnore]
    public virtual ICollection<Friend> FriendToIndividuals { get; set; } = new List<Friend>();
    [JsonIgnore]
    public virtual ICollection<LearnWord> LearnWords { get; set; } = new List<LearnWord>();
    [JsonIgnore]
    public virtual ICollection<Word> Words { get; set; } = new List<Word>();
}
