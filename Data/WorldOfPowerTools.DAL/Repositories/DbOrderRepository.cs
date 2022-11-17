using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbOrderRepository : DbRepository<Order>, IOrderRepository
    {
        public DbOrderRepository(WorldOfPowerToolsDb context) : base(context) { }

        public async Task<IEnumerable<Order>> GetByProductAndStatus(Guid productId, OrderStatus status)
        {
            var orderInStatus = await Items.Where(order => order.Status == status).ToListAsync();
            if (!orderInStatus.Any()) return Enumerable.Empty<Order>();

            var orderContainsProduct = (OrderProduct orderProduct) => orderProduct.ProductId == productId;

            var resultOrders = new List<Order>();
            foreach (var order in orderInStatus)
            {
                foreach (var orderLine in order.OrderItems)
                    if (orderContainsProduct(orderLine))
                    {
                        resultOrders.Add(order);
                        break;
                    }
            }
            return resultOrders;
        }

        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status, int skip = 0, int? take = null)
        {
            var itemsByStatus = Items.Where(product => product.Status == status);
            int skipCount = skip < 0 ? 0 : skip;
            var items = itemsByStatus.Skip(skipCount);
            if (take.HasValue && take.Value > 0)
                items = items.Take(take.Value);
            return await items.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetByUserIdAsync(Guid id)
        {
            return await Items.Where(order => order.UserId == id).ToListAsync();
        }
    }
}