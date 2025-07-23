using System;
using System.Collections.Generic;

namespace DbManagerApi.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Number { get; set; }

    public string? Email { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Friend> FriendFromIndividuals { get; set; } = new List<Friend>();

    public virtual ICollection<Friend> FriendToIndividuals { get; set; } = new List<Friend>();

    public virtual ICollection<WordsToLearn> WordsToLearns { get; set; } = new List<WordsToLearn>();
}
