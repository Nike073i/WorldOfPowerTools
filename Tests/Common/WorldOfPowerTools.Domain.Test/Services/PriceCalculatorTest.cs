using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;
using WorldOfPowerTools.Domain.Test.Models.Entities;

namespace WorldOfPowerTools.Domain.Test.Services
{
    public class PriceCalculatorTest
    {
        [Test]
        [TestCase(null, typeof(ArgumentNullException))]
        public void CreateServiceIncorrect(IProductRepository productRepository, Type awaitingException)
        {
            TestDelegate construct = () => new PriceCalculator(productRepository);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        [TestCaseSource(nameof(CalculatePriceIncorrectCases))]
        public void CalculatePriceIncorrect(IEnumerable<CartLine> cartLines, Type awaitingException)
        {
            var productRepository = new Mock<IProductRepository>();
            var calculator = new PriceCalculator(productRepository.Object);
            Assert.ThrowsAsync(awaitingException, async () => await calculator.CalculatePriceAsync(cartLines));
        }

        [Test]
        public async Task CalculatePriceCorrect()
        {
            var product1 = ProductTest.CreateProduct(price: 150d);
            var product1Id = Guid.NewGuid();
            var product2 = ProductTest.CreateProduct(price: 450d);
            var product2Id = Guid.NewGuid();
            var product3 = ProductTest.CreateProduct(price: 500d);
            var product3Id = Guid.NewGuid();
            var products = new List<CartLine>
            {
                new CartLine(product1Id, product1.Quantity),
                new CartLine(product2Id, product2.Quantity),
                new CartLine(product3Id, product3.Quantity),
            };
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup((x) => x.GetByIdAsync(product1Id)).ReturnsAsync(product1);
            productRepository.Setup((x) => x.GetByIdAsync(product2Id)).ReturnsAsync(product2);
            productRepository.Setup((x) => x.GetByIdAsync(product3Id)).ReturnsAsync(product3);

            var calculator = new PriceCalculator(productRepository.Object);
            var totalPrice = await calculator.CalculatePriceAsync(products);
            Assert.AreEqual(totalPrice, product1.Price + product2.Price + product3.Price);
        }

        static object[] CalculatePriceIncorrectCases =
        {
            new object?[] { new List<CartLine>(), typeof(ArgumentNullException) },
            new object?[] { null, typeof(ArgumentNullException) }
        };
    }
}
