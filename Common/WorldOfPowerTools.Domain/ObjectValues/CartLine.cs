namespace WorldOfPowerTools.Domain.ObjectValues
{
    public class CartLine
    {
        public Guid ProductId { get; }
        public int Quantity { get; }

#nullable disable
        protected CartLine() { }

        public CartLine(Guid productId, int quantity)
        {
            throw new System.Exception("Not implemented");
        }
    }
}