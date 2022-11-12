﻿using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.Domain.Test.Services
{
    public class SaleServiceTest
    {
        private static readonly Guid product1Id = new Guid("f1463d18-7eed-4347-999c-cb96992570b4");
        private static readonly Product product1 = new Product("product1", 150d, Category.Various, "description1", 150);

        private static readonly Guid product2Id = new Guid("1367eb11-8987-4593-843c-f287c9e372ce");
        private static Product product2 = new Product("product2", 250d, Category.Various, "description2", 250);

        private static readonly Guid product3Id = new Guid("2e9cdb6d-7b66-4a30-954c-3dea5b8a8fbc");
        private static readonly Product product3 = new Product("product3", 450d, Category.Various, "description3", 350);

        private static readonly User testUser = new User("user1", "7c6a180b36896a0a8c02787eeafb0e4c", Actions.Cart | Actions.MyOrders);
        private static readonly Guid testUserId = new Guid("2dfb2f7f-bb8f-478f-b3bf-8e1b6d5f1ac6");
        private static readonly Address testAddress = new Address("county", "sity", "stree", "flat", 100, "444444");
        private static readonly ContactData testContactData = new ContactData("+79186356458", "music@msdl.ru");

        private static IEnumerable<CartLine> testCartLines = new List<CartLine>
        {
            new CartLine(product1Id, 5),
            new CartLine(product2Id, 10),
            new CartLine(product3Id, 15)
        };

        [Test]
        [TestCaseSource(nameof(CreateServiceIncorrectCases))]
        public void CreateServiceIncorrect(PriceCalculator priceCalculator, IOrderRepository orderRepository,
            IProductRepository productRepository, IUserRepository userRepository, Type awaitingException)
        {
            TestDelegate construct = () => new SaleService(priceCalculator, orderRepository, productRepository, userRepository);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        [TestCaseSource(nameof(CreateOrderIncorrectCases))]
        public void CreateNewOrderIncorrect(Guid userId, IEnumerable<CartLine> cartLines, Address address, ContactData contactData, Type awaitingException)
        {
            var saleService = GetSaleService();
            Assert.ThrowsAsync(awaitingException, async () => await saleService.CreateOrder(userId, cartLines, address, contactData));
        }

        [Test]
        public void CreateOrderWithNonExistsUser()
        {
            var saleService = GetSaleService();
            var userId = Guid.NewGuid();
            AsyncTestDelegate badCreate = async () => await saleService.CreateOrder(userId, testCartLines, testAddress, testContactData);
            Assert.ThrowsAsync<UserNotFoundException>(badCreate);
        }

        [Test]
        public void CreateOrderWhenLittleQuantityProducts()
        {
            var saleService = GetSaleService();
            var cartLines = new List<CartLine>
            {
                new CartLine(product1Id, product1.Quantity + 1)
            };
            AsyncTestDelegate badCreate = async () => await saleService.CreateOrder(testUserId, cartLines, testAddress, testContactData);
            Assert.ThrowsAsync<InvalidOperationException>(badCreate);
        }

        [Test]
        public async Task CreateOrderCorrectValues()
        {
            var productRepository = new Mock<IProductRepository>();
            var product1Id = Guid.NewGuid();
            var product1Quantity = 10;
            var product1 = new Product("prod1", 100d, Category.Screwdriver, "description1", product1Quantity);
            var product2Id = Guid.NewGuid();
            var product2Quantity = 20;
            var product2 = new Product("prod2", 200d, Category.Various, "description2", product2Quantity);
            productRepository.Setup(x => x.GetByIdAsync(product1Id)).ReturnsAsync(product1);
            productRepository.Setup(x => x.GetByIdAsync(product2Id)).ReturnsAsync(product2);
            productRepository.Setup(x => x.SaveAsync(It.Is<Product>(p => p.Id == product1Id))).Callback<Product>(np => product1 = np).ReturnsAsync(product1);
            productRepository.Setup(x => x.SaveAsync(It.Is<Product>(p => p.Id == product2Id))).Callback<Product>(np => product2 = np).ReturnsAsync(product2);
            var priceCalculator = new Mock<PriceCalculator>(productRepository.Object);

            var saleService = GetSaleService(productRepository: productRepository.Object, priceCalculator: priceCalculator.Object);
            var cartLines = new List<CartLine>
            {
                new CartLine(product1Id, 6),
                new CartLine(product2Id, 13),
            };
            await saleService.CreateOrder(testUserId, cartLines, testAddress, testContactData);
            Assert.AreEqual(product1.Quantity, product1Quantity - cartLines[0].Quantity);
            Assert.AreEqual(product2.Quantity, product2Quantity - cartLines[1].Quantity);
        }

        [Test]
        public void CancelOrderIncorrect()
        {
            var orderId = Guid.Empty;
            var sallService = GetSaleService();
            AsyncTestDelegate badCancel = async () => await sallService.CancelOrder(orderId);
            Assert.ThrowsAsync<ArgumentNullException>(badCancel);
        }

        [Test]
        public async Task CancelOrderCorrect()
        {
            var order = new Order(testUserId, 250d, testAddress, testContactData, testCartLines);
            var orderId = Guid.NewGuid();

            var orderRepository = new Mock<IOrderRepository>();
            orderRepository.Setup(x => x.GetByIdAsync(orderId)).ReturnsAsync(order);
            orderRepository.Setup(x => x.SaveAsync(It.Is<Order>(o => o.Id == orderId))).Callback<Order>(no => order = no).ReturnsAsync(order);

            var saleService = GetSaleService(orderRepository: orderRepository.Object);
            await saleService.CancelOrder(orderId);
            Assert.True(order.Status == OrderStatus.Canceled);
        }

        private static PriceCalculator GetPriceCalculator(IProductRepository? productRepository = null)
        {
            productRepository ??= GetProductRepository();
            var priceCalculator = new Mock<PriceCalculator>(productRepository);
            return priceCalculator.Object;
        }

        private static IProductRepository GetProductRepository()
        {
            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup((x) => x.GetByIdAsync(product1Id)).ReturnsAsync(product1);
            productRepository.Setup((x) => x.GetByIdAsync(product2Id)).ReturnsAsync(product2);
            productRepository.Setup((x) => x.GetByIdAsync(product3Id)).ReturnsAsync(product3);
            return productRepository.Object;
        }

        private static IOrderRepository GetOrderRepository()
        {
            var orderRepository = new Mock<IOrderRepository>();
            return orderRepository.Object;
        }

        private static IUserRepository GetUserRepository()
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup((x) => x.GetByIdAsync(testUserId)).ReturnsAsync(testUser);
            return userRepository.Object;
        }

        private SaleService GetSaleService(PriceCalculator? priceCalculator = null, IOrderRepository? orderRepository = null,
            IProductRepository? productRepository = null, IUserRepository? userRepository = null)
        {
            productRepository ??= GetProductRepository();
            orderRepository ??= GetOrderRepository();
            userRepository ??= GetUserRepository();
            priceCalculator ??= GetPriceCalculator();
            return new SaleService(priceCalculator, orderRepository, productRepository, userRepository);
        }

        static object[] CreateServiceIncorrectCases =
        {
            new object?[] { null, GetOrderRepository(), GetProductRepository(), GetUserRepository(), typeof(ArgumentNullException)},
            new object?[] { GetPriceCalculator(), null, GetProductRepository(), GetUserRepository(), typeof(ArgumentNullException)},
            new object?[] { GetPriceCalculator(), GetOrderRepository(), null, GetUserRepository(), typeof(ArgumentNullException)},
            new object?[] { GetPriceCalculator(), GetOrderRepository(), GetProductRepository(), null,  typeof(ArgumentNullException)},
        };

        static object[] CreateOrderIncorrectCases =
{
            new object?[] { Guid.Empty, testCartLines, testAddress, testContactData, typeof(ArgumentNullException)},
            new object?[] { testUserId, null, testAddress, testContactData, typeof(ArgumentNullException)},
            new object?[] { testUserId, new List<CartLine>(), testAddress, testContactData, typeof(ArgumentNullException)},
            new object?[] { testUserId, testCartLines, null, testContactData, typeof(ArgumentNullException)},
            new object?[] { testUserId, testCartLines, testAddress, null, typeof(ArgumentNullException)},
        };
    }
}