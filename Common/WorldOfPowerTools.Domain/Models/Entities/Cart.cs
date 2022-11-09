using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class Cart
    {
        private Dictionary<Guid, CartLine> _products;

        public Cart()
        {
            throw new Exception("Not implemented");
        }
        public IEnumerable<CartLine> GetProducts()
        {
            throw new Exception("Not implemented");
        }
        public Cart AddProduct(Guid productId, int count)
        {
            throw new Exception("Not implemented");
        }
        public Cart RemoveProduct(Guid productId, int? count = null)
        {
            throw new Exception("Not implemented");
        }
        public int RemoveAll()
        {
            throw new Exception("Not implemented");
        }
    }
}
