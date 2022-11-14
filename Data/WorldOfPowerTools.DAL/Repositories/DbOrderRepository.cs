using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbOrderRepository : DbRepository<Order>, IOrderRepository
    {
        public DbOrderRepository(WorldOfPowerToolsDb context) : base(context) { }

        public Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<Order?> GetByUserIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}