using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class SaleService
    {
        private PriceCalculator _priceCalculator;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;

        public SaleService(PriceCalculator priceCalculator, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            if (priceCalculator == null) throw new ArgumentNullException(nameof(priceCalculator));
            if (productRepository == null) throw new ArgumentNullException(nameof(productRepository));
            if (orderRepository == null) throw new ArgumentNullException(nameof(orderRepository));
            if (userRepository == null) throw new ArgumentNullException(nameof(orderRepository));
            _priceCalculator = priceCalculator;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<Order> CreateOrder(Guid userId, IEnumerable<CartLine> cartLines, Address address, ContactData contactData)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (cartLines == null || !cartLines.Any()) throw new ArgumentNullException(nameof(cartLines));
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (contactData == null) throw new ArgumentNullException(nameof(contactData));

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new UserNotFoundException("Пользователь с указанным id не найден");

            var changedProducts = await RemoveProductsFromStore(cartLines);

            var totalPrice = await _priceCalculator.CalculatePriceAsync(cartLines);
            var order = new Order(userId, totalPrice, address, contactData, cartLines);
            await _productRepository.SaveRangeAsync(changedProducts);
            user.ClearCart();
            await _userRepository.SaveAsync(user);
            return await _orderRepository.SaveAsync(order);
        }

        private async Task<IEnumerable<Product>> RemoveProductsFromStore(IEnumerable<CartLine> cartLines)
        {
            var listChangedProducts = new List<Product>();
            foreach (var cartLine in cartLines)
            {
                var product = await _productRepository.GetByIdAsync(cartLine.ProductId);
                if (product == null) continue;
                if (product.Availability)
                {
                    product.RemoveFromStore(cartLine.Quantity);
                    listChangedProducts.Add(product);
                }
            }
            return listChangedProducts;
        }

        public async Task<bool> CancelOrder(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;
            order.ChangeStatus(OrderStatus.Canceled);
            await _orderRepository.SaveAsync(order);
            return true;
        }
    }
}
