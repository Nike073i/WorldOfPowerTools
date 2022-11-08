using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class SaleService
    {
        private PriceCalculator _priceCalculator;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        public SaleService(PriceCalculator priceCalculator, IOrderRepository orderRepository, IProductRepository productRepository)
        {
            throw new System.Exception("Not implemented");
        }
        public Order CreateOrder(Guid userId, IEnumerable<CartLine> cartLines, Address address, ContactData contactData)
        {
            throw new System.Exception("Not implemented");
        }
        public bool CancelOrder(Guid id)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
