using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.ObjectValues
{
    public class CartLineTest
    {
        [Test]
        [TestCaseSource(nameof(IncorrectConstructCases))]
        public void CreateCartLineIncorrect(Guid productId, int quantity, Type awaitingException)
        {
            TestDelegate construct = () => new CartLine(productId, quantity);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        public void CreateCartLineCorrect()
        {
            var productGuid = Guid.NewGuid();
            var quantity = 100;
            var expCartLine = new CartLine(productGuid, quantity);
            var cartLine = new CartLine(productGuid, quantity);
            Assert.AreEqual(expCartLine, cartLine);
        }

        static object[] IncorrectConstructCases =
        {
            new object?[] { null, 5, typeof(ArgumentNullException) },
            new object?[] { Guid.Empty, 5, typeof(ArgumentNullException) },
            new object?[] { new Guid("e9e43c3f-cb55-4877-854f-b92263948506"), -1, typeof(ArgumentOutOfRangeException) },
            new object?[] { new Guid("862fa2b0-5b56-4954-9602-3a8cea85a2c3"), 0, typeof(ArgumentOutOfRangeException) },
            new object?[] { new Guid("6e89f792-13e4-4487-8a66-3a860396526c"), 1000, typeof(ArgumentOutOfRangeException) }
        };
    }
}
