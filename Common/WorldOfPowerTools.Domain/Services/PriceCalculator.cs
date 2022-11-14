using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class PriceCalculator
    {
        private readonly IProductRepository _productRepository;

        public PriceCalculator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<double> CalculatePriceAsync(IEnumerable<CartLine> cartLines)
        {
            if (cartLines == null || !cartLines.Any()) throw new ArgumentNullException(nameof(cartLines));
            double totalPrice = 0d;
            foreach (var cartLine in cartLines)
            {
                var product = await _productRepository.GetByIdAsync(cartLine.ProductId);
                if (product != null)
                    totalPrice += product.Price;
            }
            return totalPrice;
        }
    }
}
