namespace WorldOfPowerTools.Domain.Entities
{
    public abstract class Entity<TId>
    {
        public TId Id { get; }
    }

    public abstract class Entity : Entity<Guid> { }
}