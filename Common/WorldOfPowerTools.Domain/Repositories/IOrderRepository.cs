using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Repositories
{
    public class IOrderRepository : IRepository<Order>
    {
        public Order? GetByUserIdAsync(Guid id)
        {
            throw new System.Exception("Not implemented");
        }
        public IEnumerable<Order> GetByStatusAsync(OrderStatus status, int? skip, int? take)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
