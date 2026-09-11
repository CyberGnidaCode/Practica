namespace DndPlatform.Persistence.Base;

public partial class BaseEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime DateCreate { get; set; } = DateTime.UtcNow;

    public DateTime DateUpdate { get; set; } = DateTime.UtcNow;
}