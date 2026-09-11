#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class SenderType : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
