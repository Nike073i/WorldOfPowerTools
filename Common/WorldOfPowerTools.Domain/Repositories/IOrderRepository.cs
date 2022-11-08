using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Order? GetByUserIdAsync(Guid id);
        public IEnumerable<Order> GetByStatusAsync(OrderStatus status, int? skip, int? take);
    }
}
