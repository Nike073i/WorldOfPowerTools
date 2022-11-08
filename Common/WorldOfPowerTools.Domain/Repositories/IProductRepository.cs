using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Repositories
{
    public class IProductRepository
    {
        public IEnumerable<Product> GetByCategoryAsync(Category category, int? skip, int? take)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
