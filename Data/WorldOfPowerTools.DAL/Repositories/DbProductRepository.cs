using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbProductRepository : IProductRepository
    {
        public IEnumerable<Product> GetAllAsync(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetByCategoryAsync(Category category, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Product GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid RemoveByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Product SaveAsync(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}