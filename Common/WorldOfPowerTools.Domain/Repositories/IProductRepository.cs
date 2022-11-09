using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        public IEnumerable<Product> GetByCategoryAsync(Category category, int? skip, int? take);
    }
}
