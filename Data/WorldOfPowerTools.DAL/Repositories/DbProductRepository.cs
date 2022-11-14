using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbProductRepository : DbRepository<Product>, IProductRepository
    {
        public DbProductRepository(WorldOfPowerToolsDb context) : base(context) { }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(Category category, int skip = 0, int? take = null)
        {
            var itemsByCategories = Items.Where(product => product.Category == category);
            int skipCount = skip < 0 ? 0 : skip;
            var items = itemsByCategories.Skip(skipCount);
            if (take.HasValue && take.Value > 0)
                items = items.Take(take.Value);
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<Product>> SaveRangeAsync(IEnumerable<Product> products)
        {
            using var transaction = DbContext.Database.BeginTransaction();
            {
                try
                {
                    foreach (var product in products)
                        await SaveAsync(product);
                    DbContext.SaveChanges();
                    transaction.Commit();
                    return products;
                }
                catch
                {
                    transaction.Rollback();
                    return Enumerable.Empty<Product>();
                }
            }
        }
    }
}