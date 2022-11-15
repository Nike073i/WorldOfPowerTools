using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Task<IEnumerable<Order>> GetByUserIdAsync(Guid id);
        public Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, int skip = 0, int? take = null);
        public Task<IEnumerable<Order>> GetByProductAndStatus(Guid productId, OrderStatus status);
    }
}
