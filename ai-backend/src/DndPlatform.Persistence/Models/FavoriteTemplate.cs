#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class FavoriteTemplate : BaseEntity
{
    public Guid UserId { get; set; }

    public Guid TemplateId { get; set; }

    public virtual Template Template { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
