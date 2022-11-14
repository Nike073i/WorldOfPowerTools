using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbProductRepository : DbRepository<Product>, IProductRepository
    {
        public DbProductRepository(WorldOfPowerToolsDb context) : base(context) { }

        public Task<IEnumerable<Product>> GetByCategoryAsync(Category category, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> SaveRangeAsync(IEnumerable<Product> products)
        {
            throw new NotImplementedException();
        }
    }
}