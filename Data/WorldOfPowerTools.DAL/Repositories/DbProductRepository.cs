using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbProductRepository : IProductRepository
    {
        public Task<IEnumerable<Product>> GetAllAsync(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetByCategoryAsync(Category category, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> RemoveByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Product> SaveAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}