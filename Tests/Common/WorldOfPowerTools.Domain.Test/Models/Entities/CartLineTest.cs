using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.Entities
{
    public class CartLineTest
    {
        private static readonly Guid testUserId = new Guid("e9e43c3f-cb55-4877-854f-b92263948506");
        private static readonly Guid testProductId = new Guid("e9e43c3f-cb55-4877-854f-b92263948506");
        private static readonly int testQuantity = 150;

        [Test]
        [TestCaseSource(nameof(CreateCartLineWithBadArgsCases))]
        public void CreateCartLineWithBadArgs(Guid userId, Guid productId, int quantity, Type awaitingException)
        {
            TestDelegate construct = () => new CartLine(userId, productId, quantity);
            Assert.Throws(awaitingException, construct);
        }

        static readonly object[] CreateCartLineWithBadArgsCases =
        {
            new object[] { Guid.Empty, testProductId, testQuantity, typeof(ArgumentNullException) },
            new object[] { testUserId, Guid.Empty, testQuantity, typeof(ArgumentNullException) },
            new object[] { testUserId, testProductId, CartLine.MaxProductQuantity + 1, typeof(ArgumentOutOfRangeException) },
            new object[] { testUserId, testProductId, CartLine.MinProductQuantity - 1, typeof(ArgumentOutOfRangeException) },
        };

        [Test]
        public void CreateCartLineCorrect()
        {
            var cartLine = new CartLine(testUserId, testProductId, testQuantity);
            Assert.AreEqual(cartLine.UserId, testUserId);
            Assert.AreEqual(cartLine.ProductId, testProductId);
            Assert.AreEqual(cartLine.Quantity, testQuantity);
        }

        [Test]
        [TestCaseSource(nameof(SetQuantityWithBadValueCases))]
        public void SetQuantityWithBadValue(int quantity, Type awaitingException)
        {
            var cartLine = new CartLine(testUserId, testProductId, testQuantity);
            Assert.Throws(awaitingException, () => cartLine.Quantity = quantity);
        }

        static readonly object[] SetQuantityWithBadValueCases =
        {
            new object?[] { CartLine.MaxProductQuantity + 1, typeof(ArgumentOutOfRangeException) },
            new object?[] { CartLine.MinProductQuantity - 1, typeof(ArgumentOutOfRangeException) },
        };

        [Test]
        public void SetQuantityCorrect()
        {
            var cartLine = new CartLine(testUserId, testProductId, testQuantity);
            int newQuantity = 159;
            cartLine.Quantity = newQuantity;
            Assert.AreEqual(cartLine.UserId, testUserId);
            Assert.AreEqual(cartLine.ProductId, testProductId);
            Assert.AreEqual(cartLine.Quantity, newQuantity);
        }
    }
}
