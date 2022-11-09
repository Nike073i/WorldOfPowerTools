using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        public Order? GetByUserIdAsync(Guid id);
        public IEnumerable<Order> GetByStatusAsync(OrderStatus status, int? skip, int? take);
    }
}
