using WorldOfPowerTools.Domain.ObjectValues;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Entities
{
    public class Order : Entity
    {
        private Dictionary<Guid, CartLine> _products;
        public Guid UserId { get; }
        public double Price { get; private set; }
        public Address Address { get; }
        public ContactData ContactData { get; }
        public OrderStatus Status { get; private set; }
        public DateTime DateCreated { get; }

        public Order(Guid userId, double price, Address address, ContactData contactData)
        {
            throw new System.Exception("Not implemented");
        }
        public IEnumerable<CartLine> GetProducts()
        {
            throw new System.Exception("Not implemented");
        }
        public double AddPriceSanctions(double cost)
        {
            throw new System.Exception("Not implemented");
        }
        public Order ChangeStatus(OrderStatus status)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
