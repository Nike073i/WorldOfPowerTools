using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class Order : Entity
    {
        public const double MinPrice = 1;
        public const double MaxPrice = 999_999_999d;

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
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            if (price < MinPrice || price > MaxPrice) throw new ArgumentOutOfRangeException(nameof(price));
            if (address == null) throw new ArgumentNullException(nameof(address));
            if (contactData == null) throw new ArgumentNullException(nameof(contactData));
            if (contactData == null) throw new ArgumentNullException(nameof(contactData));
            if (cartLines == null || !cartLines.Any()) throw new ArgumentNullException(nameof(cartLines));

            UserId = userId;
            Price = price;
            Address = address;
            ContactData = contactData;
            Status = OrderStatus.Created;
            DateCreated = DateTime.Now;
            _orderItems = cartLines;
        }
        public IEnumerable<CartLine> GetOrderItems()
        {
            return _orderItems;
        }
        public double AddPriceSanctions(double cost)
        {
            if (cost == 0) throw new ArgumentNullException(nameof(cost));
            double newCost = Price + cost;
            if (newCost < MinPrice || newCost > MaxPrice) throw new ArgumentOutOfRangeException(nameof(cost));
            Price += cost;
            return Price;
        }

        public Order ChangeStatus(OrderStatus status)
        {
            if (Status == OrderStatus.Canceled || Status == OrderStatus.Received) throw new InvalidOperationException("«аказ не может изменить конечное состо€ние");
            if (status == OrderStatus.Canceled
                || (Status == OrderStatus.Created && status == OrderStatus.Handled)
                || (Status == OrderStatus.Handled && status == OrderStatus.Sent)
                || (Status == OrderStatus.Sent && status == OrderStatus.Delivered)
                || (Status == OrderStatus.Delivered && status == OrderStatus.Received))
                Status = status;
            else throw new InvalidOperationException($"«аказ не может изменить состо€ние с {Status} на {status}");
            return this;
        }
    }
}