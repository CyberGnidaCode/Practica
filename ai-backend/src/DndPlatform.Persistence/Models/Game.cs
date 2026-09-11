#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class Game : BaseEntity
{
    public Guid TemplateId { get; set; }

    public Guid? GameStatusId { get; set; }

    public Guid MasterId { get; set; }

    public virtual ICollection<Character> Characters { get; set; } = new List<Character>();

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual GameStatus? GameStatus { get; set; }

    public virtual Master Master { get; set; } = null!;

    public virtual Template Template { get; set; } = null!;

    public virtual ICollection<UsersGame> UsersGames { get; set; } = new List<UsersGame>();
}
