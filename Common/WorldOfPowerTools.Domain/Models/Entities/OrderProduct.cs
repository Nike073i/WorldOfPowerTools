namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class OrderProduct : Entity
    {
        public Guid ProductId { get; protected set; }
        public int Quantity { get; protected set; }

        protected OrderProduct() { }

        public OrderProduct(Guid productId, int quantity)
        {
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
            if (quantity < 1) throw new ArgumentOutOfRangeException(nameof(quantity));

            ProductId = productId;
            Quantity = quantity;
        }
    }
}
