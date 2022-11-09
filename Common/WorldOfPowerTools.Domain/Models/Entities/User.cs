using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class User : Entity
    {
        private Cart _cart;
        public string Login { get; protected set; }
        public string PasswordHash { get; protected set; }
        public PersonData? PersonData { get; set; }
        public ContactData? ContactData { get; set; }
        public Address? Address { get; set; }
        public Actions Rights { get; protected set; }

#nullable disable
        protected User() { }

        public User(string login, string passwordHash, Actions rights)
        {
            throw new Exception("Not implemented");
        }
        public User AllowAction(Actions action)
        {
            throw new Exception("Not implemented");
        }
        public User ProhibitAction(Actions action)
        {
            throw new Exception("Not implemented");
        }
        public IEnumerable<CartLine> GetCartProducts()
        {
            throw new Exception("Not implemented");
        }
        public User AddProductInCart(Guid productId, int count)
        {
            throw new Exception("Not implemented");
        }
        public User RemoveProductFromCart(Guid productId, int? count = null)
        {
            throw new Exception("Not implemented");
        }
        public int ClearCart()
        {
            throw new Exception("Not implemented");
        }
    }
}
