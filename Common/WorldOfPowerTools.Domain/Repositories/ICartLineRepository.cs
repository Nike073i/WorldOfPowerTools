using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface ICartLineRepository : IRepository<CartLine>
    {
        public Task<IEnumerable<CartLine>> GetByUserIdAsync(Guid userId);
        public Task<int> RemoveByUserIdAsync(Guid userId);
        public Task<int> RemoveByProductIdAsync(Guid productId);
    }
}
