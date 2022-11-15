using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbCartLineRepository : DbRepository<CartLine>, ICartLineRepository
    {
        public DbCartLineRepository(WorldOfPowerToolsDb context) : base(context) { }

        public async Task<IEnumerable<CartLine>> GetByUserIdAsync(Guid userId)
        {
            return await Items.Where(line => line.UserId == userId).ToListAsync();
        }

        public async Task<int> RemoveByProductIdAsync(Guid productId)
        {
            var cartLines = Set.Where(x => x.ProductId == productId);
            return await RemoveRange(cartLines);
        }

        public async Task<int> RemoveByUserIdAsync(Guid userId)
        {
            var cartLines = Set.Where(x => x.ProductId == userId);
            return await RemoveRange(cartLines);
        }

        private async Task<int> RemoveRange(IQueryable<CartLine> cartLines)
        {
            if (!cartLines.Any()) return 0;
            Set.RemoveRange(cartLines);
            await DbContext.SaveChangesAsync();
            return cartLines.Count();
        }
    }
}
