using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class Cart
    {
        private readonly ICartLineRepository _cartLineRepository;

        public Cart(ICartLineRepository cartLineRepository)
        {
            _cartLineRepository = cartLineRepository;
        }

        public async Task<IEnumerable<CartLine>> GetUserProducts(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            return await _cartLineRepository.GetByUserIdAsync(userId);
        }

        public async Task<Cart> AddProduct(Guid userId, Guid productId, int count)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));

            int minQuantity = CartLine.MinProductQuantity;
            int maxQuantity = CartLine.MaxProductQuantity;
            if (count < minQuantity || count > maxQuantity) throw new ArgumentOutOfRangeException(nameof(count));

            int newQuantity = count;
            var userCartLines = await _cartLineRepository.GetByUserIdAsync(userId);
            var productCartLine = userCartLines.FirstOrDefault(cl => cl.ProductId == productId);
            if (productCartLine != null)
            {
                newQuantity += productCartLine.Quantity;
                newQuantity = newQuantity > maxQuantity ? maxQuantity : newQuantity;
            }
            productCartLine = new CartLine(userId, productId, newQuantity);
            await _cartLineRepository.SaveAsync(productCartLine);
            return this;
        }

        public async Task<Cart> RemoveProduct(Guid userId, Guid productId, int? count = null)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
            if (count.HasValue && count.Value < 1) throw new ArgumentOutOfRangeException(nameof(count));

            var userCartLines = await _cartLineRepository.GetByUserIdAsync(userId);
            var productCartLine = userCartLines.FirstOrDefault(cl => cl.ProductId == productId);
            if (productCartLine == null) return this;

            if (!count.HasValue)
            {
                await _cartLineRepository.RemoveByIdAsync(productCartLine.Id);
                return this;
            }

            int newQuantity = productCartLine.Quantity - count.Value;
            if (newQuantity <= 0)
            {
                await _cartLineRepository.RemoveByIdAsync(productCartLine.Id);
                return this;
            }
            productCartLine.Quantity = newQuantity;
            await _cartLineRepository.SaveAsync(productCartLine);
            return this;
        }

        public async Task<int> Clear(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            return await _cartLineRepository.RemoveByUserIdAsync(userId);
        }
    }
}
