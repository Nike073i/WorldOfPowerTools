using WorldOfPowerTools.Domain.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class PriceCalculator
    {
        private IProductRepository _productRepository;

        public PriceCalculator(IProductRepository productRepository)
        {
            throw new System.Exception("Not implemented");
        }
        public double CalculatePrice(IEnumerable<CartLine> cartLines)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
