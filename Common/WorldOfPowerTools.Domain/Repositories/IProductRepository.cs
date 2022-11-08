using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public IEnumerable<Product> GetByCategoryAsync(Category category, int? skip, int? take);
    }
}
