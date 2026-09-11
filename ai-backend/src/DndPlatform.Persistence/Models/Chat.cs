#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class Chat : BaseEntity
{
    public Guid GameId { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
