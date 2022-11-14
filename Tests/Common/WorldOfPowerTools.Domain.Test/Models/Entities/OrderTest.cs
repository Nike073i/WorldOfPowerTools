using NUnit.Framework;
using System;
using System.Collections.Generic;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.Entities
{
    internal class OrderTest
    {
        private static Guid testUserId = new Guid("e9e43c3f-cb55-4877-854f-b92263948506");
        private static double testPrice = 150d;
        private static Address testAddress = new Address("Тестовая страна", "Тестовый город", "Тестовая улица", "30б", 100, "443444");
        private static ContactData testContactData = new ContactData("+74999999999", "test@mail.ru");
        private static IEnumerable<CartLine> testCartLines = new List<CartLine> {
            new CartLine(Guid.NewGuid(), 10),
            new CartLine(Guid.NewGuid(), 20),
            new CartLine(Guid.NewGuid(), 30),
        };

        [Test]
        [TestCaseSource(nameof(IncorrectConstructCases))]
        public void CreateOrderIncorrect(Guid userId, double price, Address address, ContactData contactData, IEnumerable<CartLine> cartLines, Type awaitingException)
        {
            TestDelegate construct = () => new Order(userId, price, address, contactData, cartLines);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        public void CreateProductCorrect()
        {
            var expOrder = CreateOrder();
            var order = CreateOrder();
            Assert.True(IsEqualOrders(expOrder, order));
        }

        [Test]
        [TestCase(150, 0, typeof(ArgumentNullException))]
        [TestCase(150, -200, typeof(ArgumentOutOfRangeException))]
        [TestCase(150, Order.MaxPrice, typeof(ArgumentOutOfRangeException))]
        [TestCase(150, -Order.MaxPrice, typeof(ArgumentOutOfRangeException))]
        public void AddPriceSanctionsIncorrect(double currentPrice, double cost, Type awaitingException)
        {
            var order = CreateOrder(price: currentPrice);
            Assert.Throws(awaitingException, () => order.AddPriceSanctions(cost));
        }

        [Test]
        [TestCase(150 + Order.MinPrice, 140, 290 + Order.MinPrice)]
        [TestCase(150 + Order.MinPrice, -149, 1 + Order.MinPrice)]
        [TestCase(150, Order.MaxPrice - 150, Order.MaxPrice)]
        public void AddPriceSanctionsCorrect(double currentPrice, double cost, double newPrice)
        {
            var order = CreateOrder(price: currentPrice);
            order.AddPriceSanctions(cost);
            Assert.AreEqual(order.Price, newPrice);
        }

        [Test]
        [TestCase(OrderStatus.Created, OrderStatus.Sent, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Created, OrderStatus.Delivered, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Created, OrderStatus.Received, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Handled, OrderStatus.Created, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Handled, OrderStatus.Delivered, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Sent, OrderStatus.Created, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Sent, OrderStatus.Handled, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Sent, OrderStatus.Received, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Delivered, OrderStatus.Created, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Delivered, OrderStatus.Handled, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Delivered, OrderStatus.Sent, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Canceled, OrderStatus.Created, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Canceled, OrderStatus.Canceled, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Canceled, OrderStatus.Handled, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Canceled, OrderStatus.Sent, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Canceled, OrderStatus.Delivered, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Canceled, OrderStatus.Received, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Received, OrderStatus.Created, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Received, OrderStatus.Canceled, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Received, OrderStatus.Handled, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Received, OrderStatus.Sent, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Received, OrderStatus.Delivered, typeof(InvalidOperationException))]
        [TestCase(OrderStatus.Received, OrderStatus.Received, typeof(InvalidOperationException))]
        public void ChangeStatusIncorrect(OrderStatus currentStatus, OrderStatus newStatus, Type awaitingException)
        {
            var order = CreateOrder();
            SetOrderStatus(order, currentStatus);
            Assert.Throws(awaitingException, () => order.ChangeStatus(newStatus));
        }

        [Test]
        [TestCase(OrderStatus.Created, OrderStatus.Handled)]
        [TestCase(OrderStatus.Handled, OrderStatus.Sent)]
        [TestCase(OrderStatus.Sent, OrderStatus.Delivered)]
        [TestCase(OrderStatus.Delivered, OrderStatus.Received)]
        [TestCase(OrderStatus.Created, OrderStatus.Canceled)]
        [TestCase(OrderStatus.Handled, OrderStatus.Canceled)]
        [TestCase(OrderStatus.Sent, OrderStatus.Canceled)]
        [TestCase(OrderStatus.Delivered, OrderStatus.Canceled)]        
        public void ChangeStatusCorrect(OrderStatus currentStatus, OrderStatus newStatus)
        {
            var order = CreateOrder();
            SetOrderStatus(order, currentStatus);
            order.ChangeStatus(newStatus);
            Assert.AreEqual(order.Status, newStatus);
        }

        private Order CreateOrder(Guid? userId = null, double price = 100d, Address? address = null, ContactData? contactData = null, IEnumerable<CartLine>? cartLines = null)
        {
            userId ??= testUserId;
            address ??= testAddress;
            contactData ??= testContactData;
            cartLines ??= testCartLines;
            return new Order(userId.Value, price, address, contactData, cartLines);
        }

        private bool IsEqualOrders(Order a, Order b)
        {
            return a.UserId == b.UserId &&
                a.Price == b.Price &&
                a.Address == b.Address &&
                a.ContactData == b.ContactData &&
                a.Status == b.Status;
        }

        private void SetOrderStatus(Order order, OrderStatus newStatus)
        {
            if (newStatus == OrderStatus.Canceled)
                order.ChangeStatus(newStatus);
            while (order.Status != newStatus)
            {
                switch (order.Status)
                {
                    case OrderStatus.Created:
                        order.ChangeStatus(OrderStatus.Handled);
                        break;
                    case OrderStatus.Handled:
                        order.ChangeStatus(OrderStatus.Sent);
                        break;
                    case OrderStatus.Sent:
                        order.ChangeStatus(OrderStatus.Delivered);
                        break;
                    case OrderStatus.Delivered:
                        order.ChangeStatus(OrderStatus.Received);
                        break;
                }
            }

        }

        static readonly object[] IncorrectConstructCases =
{
            new object?[] { Guid.Empty, testPrice, testAddress, testContactData, testCartLines, typeof(ArgumentNullException)},
            new object?[] { testUserId, Order.MinPrice - 1, testAddress, testContactData, testCartLines, typeof(ArgumentOutOfRangeException)},
            new object?[] { testUserId, Order.MaxPrice + 1, testAddress, testContactData, testCartLines, typeof(ArgumentOutOfRangeException)},
            new object?[] { testUserId, testPrice, testAddress, testContactData, null, typeof(ArgumentNullException)},
            new object?[] { testUserId, testPrice, testAddress, testContactData, new List<CartLine>(), typeof(ArgumentNullException)}
        };
    }
}
