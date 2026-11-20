namespace CVParser.Domain.Entities;

public abstract class EntityBase
{
    public Guid Id { get; protected set; }
    public DateTime CreatedDate { get; protected set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; protected set; } = DateTime.UtcNow;
}
