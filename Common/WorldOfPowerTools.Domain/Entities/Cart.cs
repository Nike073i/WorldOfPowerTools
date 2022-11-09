using WorldOfPowerTools.Domain.ObjectValues;

namespace WorldOfPowerTools.Domain.Entities
{
    public class Cart
    {
        private Dictionary<Guid, CartLine> _products;

        public Cart()
        {
            throw new System.Exception("Not implemented");
        }
        public IEnumerable<CartLine> GetProducts()
        {
            throw new System.Exception("Not implemented");
        }
        public Cart AddProduct(Guid productId, int count)
        {
            throw new System.Exception("Not implemented");
        }
        public Cart RemoveProduct(Guid productId, int? count = null)
        {
            throw new System.Exception("Not implemented");
        }
        public int RemoveAll()
        {
            throw new System.Exception("Not implemented");
        }
    }
}
