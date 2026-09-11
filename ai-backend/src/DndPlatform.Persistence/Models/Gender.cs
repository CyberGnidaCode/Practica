#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class Gender : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<CharacterTemplate> CharacterTemplates { get; set; } = new List<CharacterTemplate>();
}
