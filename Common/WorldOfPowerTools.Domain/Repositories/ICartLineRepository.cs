using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface ICartLineRepository : IRepository<CartLine>
    {
        public Task<IEnumerable<CartLine>> GetByUserId(Guid userId);
        public Task<int> RemoveByUserId(Guid userId);
        public Task<int> RemoveByProductId(Guid productId);
    }
}
