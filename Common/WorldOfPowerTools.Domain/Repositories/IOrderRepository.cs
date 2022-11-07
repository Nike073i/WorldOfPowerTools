using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Repositories
{
    public class IOrderRepository : IRepository<Order>
    {
        public Order? GetByUserId(ref Guid id)
        {
            throw new System.Exception("Not implemented");
        }
        public IEnumerable<Order> GetByStatus(OrderStatus status, int? skip, int? take)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
