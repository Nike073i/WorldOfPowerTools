using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class Cart
    {
        public static readonly int MaxProductQuantity = 999;

        private Dictionary<Guid, CartLine> _products;

        public Cart()
        {
            _products = new();
        }

        public IEnumerable<CartLine> GetProducts()
        {
            return _products.Values;
        }

        public Cart AddProduct(Guid productId, int count)
        {
            if (productId == Guid.Empty) throw new ArgumentNullException(nameof(productId));
            if (count <= 0 || count > MaxProductQuantity) throw new ArgumentOutOfRangeException(nameof(count));

            int newQuantity = count;
            if (_products.ContainsKey(productId))
            {
                newQuantity += _products[productId].Quantity;
                newQuantity = newQuantity > MaxProductQuantity ? MaxProductQuantity : newQuantity;
            }
            _products[productId] = new CartLine(productId, newQuantity);
            return this;
        }

        public Cart RemoveProduct(Guid productId, int? count = null)
        {
            var line = _products[productId];
            if (line == null) return this;

            if (count == null)
            {
                _products.Remove(productId);
                return this;
            }

            if (count < 1) throw new ArgumentOutOfRangeException(nameof(count));
            int newQuantity = line.Quantity - count.Value;
            if (newQuantity <= 0)
            {
                _products.Remove(productId);
                return this;
            }
            _products[productId] = new CartLine(productId, newQuantity);
            return this;
        }

        public int RemoveAll()
        {
            int count = _products.Count;
            _products.Clear();
            return count;
        }
    }
}
