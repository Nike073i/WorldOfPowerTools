using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Models.ObjectValues
{
    public class CartLine : Entity
    {
        public static readonly int MaxProductQuantity = 999;
        public static readonly int MinProductQuantity = 1;

        public Guid UserId { get; protected set; }
        public Guid ProductId { get; protected set; }

        private int _quantity;
        public int Quantity
        {
            get => _quantity;
            set
            {
                ThrowExceptionIfQuantityIncorrect(value);
                _quantity = value;
            }
        }

        protected CartLine() { }

        public CartLine(Guid userId, Guid productId, int quantity)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
            ThrowExceptionIfQuantityIncorrect(quantity);

            UserId = userId;
            ProductId = productId;
            Quantity = quantity;
        }

        private void ThrowExceptionIfQuantityIncorrect(int quantity)
        {
            if (quantity < MinProductQuantity || quantity > MaxProductQuantity) throw new ArgumentOutOfRangeException(nameof(quantity));
        }
    }
}