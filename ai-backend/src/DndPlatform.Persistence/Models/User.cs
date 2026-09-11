#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class User : BaseEntity
{
    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public decimal Balance { get; set; }

    public virtual ICollection<CharacterTemplate> CharacterTemplates { get; set; } = new List<CharacterTemplate>();

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<FavoriteTemplate> FavoriteTemplates { get; set; } = new List<FavoriteTemplate>();

    public virtual ICollection<UsersGame> UsersGames { get; set; } = new List<UsersGame>();
}
