namespace WorldOfPowerTools.Domain.Models.ObjectValues
{
    public class CartLine
    {
        public Guid ProductId { get; protected set; }
        public int Quantity { get; protected set; }

#nullable disable
        protected CartLine() { }

        public CartLine(Guid productId, int quantity)
        {
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
            if (quantity < 1 || quantity > 999) throw new ArgumentOutOfRangeException(nameof(quantity));

            ProductId = productId;
            Quantity = quantity;
        }

        public override bool Equals(object obj)
        {
            return (obj is CartLine other) && Equals(other);
        }

        public bool Equals(CartLine other)
        {
            if (other == null) return false;
            return ProductId == other.ProductId &&
                Quantity == other.Quantity;
        }

        public override int GetHashCode()
        {
            return ProductId.GetHashCode();
        }

        public override string ToString()
        {
            return $"{ProductId} - {Quantity}";
        }
    }
}