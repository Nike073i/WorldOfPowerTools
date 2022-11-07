namespace WorldOfPowerTools.Domain.ObjectValues
{
    public class CartLine
    {
        public Guid ProductId { get; }
        public int Quantity { get; }

        public CartLine(Guid productId, int quantity)
        {
            throw new System.Exception("Not implemented");
        }
    }
}