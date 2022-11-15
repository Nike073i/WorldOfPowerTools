using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Test.Models.Entities
{
    public class OrderProductTests
    {
        private static int testQuantity = 150;
        private static Guid testProductId = new Guid("ca8ff60f-373c-4119-9555-174c5d6ef051");

        [Test]
        [TestCaseSource(nameof(CreateOrderProductWithBadArgsCases))]
        public void CreateOrderProductWithBadArgs(Guid productId, int quantity, Type awaitingException)
        {
            TestDelegate construct = () => new OrderProduct(productId, quantity);
            Assert.Throws(awaitingException, construct);
        }

        static readonly object[] CreateOrderProductWithBadArgsCases =
        {
            new object[] { Guid.Empty, testQuantity, typeof(ArgumentNullException) },
            new object[] { testProductId, -1, typeof(ArgumentOutOfRangeException) },
            new object[] { testProductId, 0, typeof(ArgumentOutOfRangeException) }
        };

        [Test]
        public void CreateOrderProductCorrect()
        {
            OrderProduct orderProduct = new OrderProduct(testProductId, testQuantity);
            Assert.NotNull(orderProduct);
            Assert.AreEqual(orderProduct.Quantity, testQuantity);
            Assert.AreEqual(orderProduct.ProductId, testProductId);
        }
    }
}
