#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Base;

namespace DndPlatform.Persistence.Models;

public partial class Template : BaseEntity
{
    public string Name { get; set; } = null!;

    public string Desription { get; set; } = null!;

    public string Setting { get; set; } = null!;

    public string RedFlag { get; set; } = null!;

    public bool? IsPublic { get; set; }

    public virtual ICollection<FavoriteTemplate> FavoriteTemplates { get; set; } = new List<FavoriteTemplate>();

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();

    public virtual ICollection<TemplatesGenre> TemplatesGenres { get; set; } = new List<TemplatesGenre>();
}
