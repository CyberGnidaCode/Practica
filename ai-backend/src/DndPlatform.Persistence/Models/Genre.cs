#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class Genre : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<TemplatesGenre> TemplatesGenres { get; set; } = new List<TemplatesGenre>();
}
