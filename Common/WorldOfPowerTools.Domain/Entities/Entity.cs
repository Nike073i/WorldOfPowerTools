namespace WorldOfPowerTools.Domain.Entities
{
    public abstract class Entity<TId>
    {
        public TId? Id { get; protected set; }
    }

    public abstract class Entity : Entity<Guid> { }
}