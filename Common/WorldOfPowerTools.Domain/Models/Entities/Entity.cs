namespace WorldOfPowerTools.Domain.Models.Entities
{
    public abstract class Entity<TId>
    {
        public TId? Id { get; protected set; }
    }

    public abstract class Entity : Entity<Guid> { }
}