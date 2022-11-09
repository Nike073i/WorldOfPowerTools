using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class Order : Entity
    {
        private IEnumerable<CartLine> _orderItems;
        public Guid UserId { get; protected set; }
        public double Price { get; protected set; }
        public Address Address { get; protected set; }
        public ContactData ContactData { get; protected set; }
        public OrderStatus Status { get; protected set; }
        public DateTime DateCreated { get; protected set; }

#nullable disable
        protected Order() { }

        public Order(Guid userId, double price, Address address, ContactData contactData, IEnumerable<CartLine> cartLines)
        {
            throw new Exception("Not implemented");
        }
        public IEnumerable<CartLine> GetOrderItems()
        {
            throw new Exception("Not implemented");
        }
        public double AddPriceSanctions(double cost)
        {
            throw new Exception("Not implemented");
        }
        public Order ChangeStatus(OrderStatus status)
        {
            throw new Exception("Not implemented");
        }
    }
}
