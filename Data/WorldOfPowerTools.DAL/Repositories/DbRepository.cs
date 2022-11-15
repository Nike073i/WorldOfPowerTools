using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public abstract class DbRepository<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        protected readonly WorldOfPowerToolsDb DbContext;
        protected readonly DbSet<TEntity> Set;

        protected virtual IQueryable<TEntity> Items => Set;

        protected DbRepository(WorldOfPowerToolsDb context)
        {
            DbContext = context;
            Set = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(int skip = 0, int? take = null)
        {
            int skipCount = skip < 0 ? 0 : skip;
            var items = Items.Skip(skipCount);
            if (take.HasValue && take.Value > 0)
                items = items.Take(take.Value);
            return await items.ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(Guid id)
        {
            return await Items.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Guid> RemoveByIdAsync(Guid id)
        {
            var entity = await Set.FirstOrDefaultAsync(x => x.Id == id);
            if (entity == null) throw new EntityNotFoundException("Ошибка удаления по Id: Запись не найдена");
            DbContext.Entry(entity).State = EntityState.Deleted;
            await DbContext.SaveChangesAsync();
            return id;
        }

        public async Task<TEntity> SaveAsync(TEntity entity)
        {
            var storedEntity = await Set.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (storedEntity == null)
                await Set.AddAsync(entity);
            else
                Set.Update(entity);
            await DbContext.SaveChangesAsync();
            return entity;
        }
    }
}