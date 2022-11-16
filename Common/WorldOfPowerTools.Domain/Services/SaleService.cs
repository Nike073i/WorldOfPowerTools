using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class SaleService
    {
        private readonly PriceCalculator _priceCalculator;
        private readonly Cart _cart;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;

        public SaleService(PriceCalculator priceCalculator, Cart cart, IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _priceCalculator = priceCalculator;
            _cart = cart;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _userRepository = userRepository;
        }

        public async Task<Order> CreateOrder(Guid userId, IEnumerable<CartLine> cartLines, Address address, ContactData contactData)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (!cartLines.Any()) throw new ArgumentNullException(nameof(cartLines));

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) throw new EntityNotFoundException("Пользователь с указанным id не найден");

            var totalPrice = await _priceCalculator.CalculatePriceAsync(cartLines);
            var order = new Order(userId, totalPrice, address, contactData, cartLines);
            await _cart.Clear(userId);
            await _userRepository.SaveAsync(user);
            return await _orderRepository.SaveAsync(order);
        }

        public async Task<Order> ConfirmOrder(Guid orderId)
        {
            if (orderId == Guid.Empty) throw new ArgumentNullException(nameof(orderId));
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new EntityNotFoundException("Заказ не найден");
            await UpdateOrderStatus(orderId, OrderStatus.Created, OrderStatus.Handled);
            var changedProducts = await RemoveProductsFromStore(order.OrderItems);
            await _productRepository.SaveRangeAsync(changedProducts);
            return order;
        }

        public async Task<Order> SendOrder(Guid orderId)
        {
            return await UpdateOrderStatus(orderId, OrderStatus.Handled, OrderStatus.Sent);
        }

        public async Task<Order> DeliveOrder(Guid orderId)
        {
            return await UpdateOrderStatus(orderId, OrderStatus.Sent, OrderStatus.Delivered);
        }

        public async Task<Order> ReceiveOrder(Guid orderId)
        {
            return await UpdateOrderStatus(orderId, OrderStatus.Delivered, OrderStatus.Received);
        }

        private async Task<Order> UpdateOrderStatus(Guid orderId, OrderStatus current, OrderStatus newStatus)
        {
            if (orderId == Guid.Empty) throw new ArgumentNullException(nameof(orderId));
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null) throw new EntityNotFoundException("Заказ не найден");
            if (order.Status != current) throw new OrderChangeStatusException($"Заказ не в состоянии -{current}");
            order.ChangeStatus(newStatus);
            await _orderRepository.SaveAsync(order);
            return order;
        }

        private async Task<IEnumerable<Product>> RemoveProductsFromStore(IEnumerable<OrderProduct> orderProducts)
        {
            var listChangedProducts = new List<Product>();
            foreach (var cartLine in orderProducts)
            {
                var product = await _productRepository.GetByIdAsync(cartLine.ProductId);
                if (product == null) continue;
                if (!product.Availability) throw new OrderCouldNotBeCreatedException($"Товар {product.Name} недоступен к продаже");
                {
                    product.RemoveFromStore(cartLine.Quantity);
                    listChangedProducts.Add(product);
                }
            }
            return listChangedProducts;
        }

        private async Task<bool> CancelCreatedOrder(Guid orderId)
        {
            await _orderRepository.RemoveByIdAsync(orderId);
            return true;
        }

        public async Task<bool> CancelOrder(Guid id)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return false;
            if (order.Status == OrderStatus.Canceled || order.Status == OrderStatus.Received) throw new OrderChangeStatusException("Заказ невозмножно отменить");
            if (order.Status == OrderStatus.Created) return await CancelCreatedOrder(id);
            order.ChangeStatus(OrderStatus.Canceled);
            var listChangedProducts = await RestoreProducts(order.OrderItems);
            await _productRepository.SaveRangeAsync(listChangedProducts);
            await _orderRepository.SaveAsync(order);
            return true;
        }

        private async Task<IEnumerable<Product>> RestoreProducts(IEnumerable<OrderProduct> cartLines)
        {
            var listChangedProducts = new List<Product>();
            foreach (var cartLine in cartLines)
            {
                var product = await _productRepository.GetByIdAsync(cartLine.ProductId);
                if (product == null) continue;
                product.AddToStore(cartLine.Quantity);
                listChangedProducts.Add(product);
            }
            return listChangedProducts;
        }
    }
}
