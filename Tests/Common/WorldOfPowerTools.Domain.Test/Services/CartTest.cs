using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.Domain.Test.Services
{
    public class CartTest
    {
        private static readonly Guid testProductId = new Guid("e9e43c3f-cb55-4877-854f-b92263948506");
        private static readonly Guid testUserId = new Guid("e9e43c3f-cb55-4877-854f-b92263948506");
        private static readonly int testQuantity = 150;

        [Test]
        [TestCaseSource(nameof(AddProductNewWithBadArgsCases))]
        public void AddProductNewWithBadArgs(Guid userId, Guid productId, int quantity, Type awaitingException)
        {
            var cart = CreateCart();
            AsyncTestDelegate badInvoke = async () => await cart.AddProduct(userId, productId, quantity);
            Assert.ThrowsAsync(awaitingException, badInvoke);
        }

        static readonly object[] AddProductNewWithBadArgsCases =
        {
            new object[] { Guid.Empty, testProductId, testQuantity, typeof(ArgumentNullException) },
            new object[] { testUserId, Guid.Empty, testQuantity, typeof(ArgumentNullException) },
            new object[] { testUserId, testProductId, CartLine.MaxProductQuantity + 1, typeof(ArgumentOutOfRangeException) },
            new object[] { testUserId, testProductId, CartLine.MinProductQuantity - 1, typeof(ArgumentOutOfRangeException) },
        };

        [Test]
        public async Task AddProductNew()
        {
            CartLine? cartLine = null;
            var cartLineRepository = new Mock<ICartLineRepository>();
            cartLineRepository.Setup(x => x.GetByUserIdAsync(testUserId)).ReturnsAsync(Enumerable.Empty<CartLine>);
            cartLineRepository.Setup(x => x.SaveAsync(It.Is<CartLine>(o => o.ProductId == testProductId))).Callback<CartLine>(ncl => cartLine = ncl).ReturnsAsync(cartLine);
            var cart = CreateCart(cartLineRepository.Object);

            await cart.AddProduct(testUserId, testProductId, testQuantity);

            Assert.NotNull(cartLine);
            Assert.AreEqual(cartLine.ProductId, testProductId);
            Assert.AreEqual(cartLine.Quantity, testQuantity);
            Assert.AreEqual(cartLine.UserId, testUserId);
        }

        [Test]
        [TestCaseSource(nameof(AddProductAdditionCases))]
        public async Task AddProductAddition(int quantity, int addition, int newQuantity)
        {
            CartLine cartLine = new CartLine(testUserId, testProductId, quantity);
            var cartLineRepository = new Mock<ICartLineRepository>();
            cartLineRepository.Setup(x => x.GetByUserIdAsync(testUserId)).ReturnsAsync(Enumerable.Repeat(cartLine, 1));
            cartLineRepository.Setup(x => x.SaveAsync(It.Is<CartLine>(o => o.ProductId == testProductId))).Callback<CartLine>(ncl => cartLine = ncl).ReturnsAsync(cartLine);
            var cart = CreateCart(cartLineRepository.Object);

            await cart.AddProduct(testUserId, testProductId, addition);

            Assert.NotNull(cartLine);
            Assert.AreEqual(cartLine.ProductId, testProductId);
            Assert.AreEqual(cartLine.Quantity, newQuantity);
            Assert.AreEqual(cartLine.UserId, testUserId);
        }

        static readonly object[] AddProductAdditionCases =
        {
            new object[] { 15, 10, 25 },
            new object[] { 1, 100, 101 },
            new object[] { 100, CartLine.MaxProductQuantity, CartLine.MaxProductQuantity }
        };

        [Test]
        [TestCaseSource(nameof(RemoveProductWithBadArgsCases))]
        public void RemoveProductWithBadArgs(Guid userId, Guid productId, int? quantity, Type awaitingException)
        {
            var cart = CreateCart();
            AsyncTestDelegate badInvoke = async () => await cart.RemoveProduct(userId, productId, quantity);
            Assert.ThrowsAsync(awaitingException, badInvoke);
        }

        static readonly object[] RemoveProductWithBadArgsCases =
        {
            new object[] { Guid.Empty, testProductId, testQuantity, typeof(ArgumentNullException) },
            new object[] { testUserId, Guid.Empty, testQuantity, typeof(ArgumentNullException) },
            new object[] { testUserId, testProductId, -1, typeof(ArgumentOutOfRangeException) },
            new object[] { testUserId, testProductId, 0, typeof(ArgumentOutOfRangeException) },
        };

        [Test]
        [TestCase(100, 50, 50)]
        public async Task RemoveProduct(int quantity, int? removal, int? newQuantity)
        {
            var createdLine = new CartLine(testUserId, testProductId, quantity);
            var cartLineRepository = new Mock<ICartLineRepository>();
            cartLineRepository.Setup(x => x.GetByUserIdAsync(testUserId)).ReturnsAsync(Enumerable.Repeat(createdLine, 1));
            cartLineRepository.Setup(x => x.SaveAsync(It.Is<CartLine>(o => o.ProductId == testProductId))).Callback<CartLine>(ncl => createdLine = ncl).ReturnsAsync(createdLine);
            var cart = CreateCart(cartLineRepository.Object);

            await cart.RemoveProduct(testUserId, testProductId, removal);

            if (newQuantity.HasValue) Assert.IsNotNull(createdLine);
            Assert.AreEqual(createdLine.ProductId, testProductId);
            Assert.AreEqual(createdLine.Quantity, newQuantity);
            Assert.AreEqual(createdLine.UserId, testUserId);
        }

        [Test]
        public async Task RemoveAll()
        {
            var cartLine = new CartLine(testUserId, testProductId, testQuantity);
            var cartLineCount = 5;
            var cartLines = Enumerable.Repeat(cartLine, cartLineCount);

            var cartLineRepository = new Mock<ICartLineRepository>();
            cartLineRepository.Setup(x => x.RemoveByUserIdAsync(testUserId)).Callback<Guid>(ncl => cartLines = Enumerable.Empty<CartLine>()).ReturnsAsync(cartLineCount);
            var cart = CreateCart(cartLineRepository.Object);

            var countRemoved = await cart.Clear(testUserId);
            Assert.False(cartLines.Any());
            Assert.AreEqual(countRemoved, cartLineCount);
        }

        private ICartLineRepository GetCartLineRepository()
        {
            var mock = new Mock<ICartLineRepository>();
            return mock.Object;
        }

        private Cart CreateCart(ICartLineRepository? cartLineRepository = null)
        {
            cartLineRepository ??= GetCartLineRepository();
            var cart = new Cart(cartLineRepository);
            return cart;
        }
    }
}
