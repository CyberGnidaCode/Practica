#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class Message : BaseEntity
{
    public Guid ChatId { get; set; }

    public Guid? UserId { get; set; }

    public Guid SenderTypeId { get; set; }

    public string Text { get; set; } = null!;

    public virtual Chat Chat { get; set; } = null!;

    public virtual SenderType SenderType { get; set; } = null!;
}
