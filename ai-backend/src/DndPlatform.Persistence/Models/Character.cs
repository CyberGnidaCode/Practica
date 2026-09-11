#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class Character : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Race { get; set; } = null!;

    public int Hp { get; set; }

    public int Power { get; set; }

    public int Agility { get; set; }

    public int Physique { get; set; }

    public int Intelligence { get; set; }

    public int Wisdom { get; set; }

    public int Charisma { get; set; }

    public Guid UserId { get; set; }

    public Guid GameId { get; set; }

    public virtual Game Game { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
