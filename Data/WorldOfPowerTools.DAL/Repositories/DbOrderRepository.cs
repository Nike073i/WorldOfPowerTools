using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbOrderRepository : IOrderRepository
    {
        public IEnumerable<Order> GetAllAsync(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Order GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetByStatusAsync(OrderStatus status, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Order GetByUserIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Guid RemoveByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Order SaveAsync(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}