using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.ObjectValues;

namespace WorldOfPowerTools.Domain.Entities
{
    public class User : Entity
    {
        private Cart _cart;
        public string Login { get; protected set; }
        public string PasswordHash { get; protected set; }
        public PersonData? PersonData { get; protected set; }
        public ContactData? ContactData { get; protected set; }
        public Address? Address { get; protected set; }
        public Actions Rights { get; protected set; }

#nullable disable
        protected User() { }

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
