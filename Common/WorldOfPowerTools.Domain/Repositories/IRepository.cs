using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IRepository<TId, TEntity> where TEntity : Entity<TId>
    {
        public Task<TEntity> SaveAsync(TEntity entity);
        public Task<TId> RemoveByIdAsync(TId id);
        public Task<IEnumerable<TEntity>> GetAllAsync(int skip = 0, int? take = null);
        public Task<TEntity?> GetByIdAsync(TId id);
    }

    public interface IRepository<TEntity> : IRepository<Guid, TEntity> where TEntity : Entity<Guid> { }
}
