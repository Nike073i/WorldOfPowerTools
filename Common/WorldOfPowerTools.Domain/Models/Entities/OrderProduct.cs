namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class OrderProduct : Entity
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        protected OrderProduct() { }
        public OrderProduct(Guid productId, int quantity)
        {
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
        }
    }
}
