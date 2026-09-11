#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class TemplatesGenre : BaseEntity
{
    public Guid TemplateId { get; set; }

    public Guid GenreId { get; set; }

    public virtual Genre Genre { get; set; } = null!;

    public virtual Template Template { get; set; } = null!;
}
