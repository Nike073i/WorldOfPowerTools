using WorldOfPowerTools.Domain.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IRepository<TId, TEntity> where TEntity : Entity<TId>
    {
        public TEntity SaveAsync(TEntity entity);
        public TId RemoveByIdAsync(TId id);
        public IEnumerable<TEntity> GetAllAsync(int? skip, int? take);
        public TEntity? GetByIdAsync(TId id);
    }

    public interface IRepository<TEntity> : IRepository<Guid, TEntity> where TEntity : Entity<Guid> { }
}
