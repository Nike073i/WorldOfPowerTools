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
            throw new Exception("Not implemented");
        }
    }
}