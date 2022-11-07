using WorldOfPowerTools.Domain.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public class IRepository<TId, TEntity> where TEntity : Entity<TId>
    {
        public TEntity SaveAsync(TEntity entity)
        {
            throw new System.Exception("Not implemented");
        }
        public TId RemoveByIdAsync(TId id)
        {
            throw new System.Exception("Not implemented");
        }
        public IEnumerable<TEntity> GetAllAsync(int? skip, int? take)
        {
            throw new System.Exception("Not implemented");
        }
        public TEntity? GetByIdAsync(TId id)
        {
            throw new System.Exception("Not implemented");
        }
    }

    public class IRepository<TEntity> : IRepository<Guid, TEntity> where TEntity : Entity<Guid> { }
}
