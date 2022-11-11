using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.Domain.Test.Models.Entities
{
    public class CartTest
    {
        [Test]
        [TestCaseSource(nameof(AddProductIncorrectValueCases))]
        public void AddProductIncorrectValue(Guid productId, int count, Type awaitingException)
        {
            var cart = new Cart();
            Assert.Throws(awaitingException, () => cart.AddProduct(productId, count));
        }

        [Test]
        public void AddProductNewValue()
        {
            var cart = new Cart();
            var productId = Guid.NewGuid();
            var quantity = 100;
            cart.AddProduct(productId, quantity);
            var cartProducts = cart.GetProducts();
            Assert.True(IsContainsProduct(productId, cartProducts, quantity));
        }

        [Test]
        [TestCaseSource(nameof(AddProductAdditionValueCases))]
        public void AddProductAdditionValue(int quantity, int addition, int newQuantity)
        {
            var cart = new Cart();
            var productId = Guid.NewGuid();
            cart.AddProduct(productId, quantity).AddProduct(productId, addition);
            var cartProducts = cart.GetProducts();
            Assert.True(IsContainsProduct(productId, cartProducts, newQuantity));
        }

        [Test]
        [TestCase(-1, typeof(ArgumentOutOfRangeException))]
        [TestCase(0, typeof(ArgumentOutOfRangeException))]
        public void RemoveProductIncorrectValue(int quantity, Type awaitingException)
        {
            var cart = new Cart();
            var productId = Guid.NewGuid();
            cart.AddProduct(productId, 100);
            Assert.Throws(awaitingException, () => cart.RemoveProduct(productId, quantity));
        }

        [Test]
        [TestCase(100, 50, 50)]
        [TestCase(100, 150, null)]
        [TestCase(100, null, null)]
        public void RemoveProductValue(int quantity, int? removal, int? newQuantity)
        {
            var cart = new Cart();
            var productId = Guid.NewGuid();
            cart.AddProduct(productId, quantity);
            cart.RemoveProduct(productId, removal);
            var cartProducts = cart.GetProducts();
            var predicate = newQuantity == null ? !IsContainsProduct(productId, cartProducts)
                : IsContainsProduct(productId, cartProducts, newQuantity);
            Assert.True(predicate);
        }

        [Test]
        public void RemoveAll()
        {
            var cart = new Cart();
            cart.AddProduct(Guid.NewGuid(), 100);
            cart.AddProduct(Guid.NewGuid(), 100);
            cart.RemoveAll();
            var products = cart.GetProducts();
            Assert.False(products.Any());
        }

        private bool IsContainsProduct(Guid productId, IEnumerable<CartLine> cartLines, int? quantity = null)
        {
            foreach (var cartLine in cartLines)
            {
                if (cartLine.ProductId == productId)
                    return quantity == null || cartLine.Quantity == quantity;
            }
            return false;
        }

        static readonly object[] AddProductIncorrectValueCases =
        {
            new object?[] { Guid.Empty, 12, typeof(ArgumentNullException) },
            new object?[] { new Guid("e9e43c3f-cb55-4877-854f-b92263948506"), -1, typeof(ArgumentOutOfRangeException) },
            new object?[] { new Guid("e9e43c3f-cb55-4877-854f-b92263948506"), 0, typeof(ArgumentOutOfRangeException) },
            new object?[] { new Guid("e9e43c3f-cb55-4877-854f-b92263948506"), Cart.MaxProductQuantity + 1, typeof(ArgumentOutOfRangeException) },
        };

        static readonly object[] AddProductAdditionValueCases =
        {
            new object[] { 15, 10, 25 },
            new object[] { 1, 100, 101 },
            new object[] { 100, Cart.MaxProductQuantity, Cart.MaxProductQuantity }
        };
    }
}
