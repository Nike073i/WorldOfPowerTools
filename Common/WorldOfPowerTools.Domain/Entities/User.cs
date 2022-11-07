using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.ObjectValues;

namespace WorldOfPowerTools.Domain.Entities
{
    public class User : Entity
    {
        private Cart _cart;
        public string Login { get; private set; }
        public string PasswordHash { get; private set; }
        public PersonData? PersonData { get; set; }
        public ContactData? ContactData { get; set; }
        public Address? Address { get; set; }
        public Actions Rights { get; private set; }

        public User(string login, string passwordHash, Actions rights)
        {
            throw new System.Exception("Not implemented");
        }
        public User AllowAction(Actions action)
        {
            throw new System.Exception("Not implemented");
        }
        public User ProhibitAction(Actions action)
        {
            throw new System.Exception("Not implemented");
        }
        public IEnumerable<CartLine> GetCartProducts()
        {
            throw new System.Exception("Not implemented");
        }
        public User AddProductInCart(Guid productId, int count)
        {
            throw new System.Exception("Not implemented");
        }
        public User RemoveProductFromCart(Guid productId, int? count)
        {
            throw new System.Exception("Not implemented");
        }
        public int ClearCart()
        {
            throw new System.Exception("Not implemented");
        }
    }
}
