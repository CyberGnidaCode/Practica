#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class GameStatus : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
