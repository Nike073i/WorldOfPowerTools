using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbOrderRepository : IOrderRepository
    {
        public Task<IEnumerable<Order>> GetAllAsync(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByUserIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> RemoveByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Order> SaveAsync(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}