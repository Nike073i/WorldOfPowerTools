using WorldOfPowerTools.Domain.ObjectValues;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Entities
{
    public class Order : Entity
    {
        private Dictionary<Guid, CartLine> _products;
        public Guid UserId { get; protected set; }
        public double Price { get; protected set; }
        public Address Address { get; protected set; }
        public ContactData ContactData { get; protected set; }
        public OrderStatus Status { get; protected set; }
        public DateTime DateCreated { get; protected set; }

#nullable disable
        protected Order() { }

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
