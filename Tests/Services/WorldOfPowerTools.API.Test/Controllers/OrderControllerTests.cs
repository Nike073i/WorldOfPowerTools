using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldOfPowerTools.API.Controllers;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.ValueObject;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Authorization;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Controllers;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Requests;
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Test.Controllers
{
    public class OrderControllerTests : ControllerBaseTests
    {
        [Test]
        public async Task GetAllOk()
        {
            await InitializeData();

            var orderController = GetOrderController();

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.GetAllAccess);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.GetAll());
            var orders = objectResult!.Value as IEnumerable<Order>;
            Assert.True(orders!.Any());
        }

        [Test]
        public async Task GetAllNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotAllowedRequest(async () => await orderController.GetAll());
        }

        [Test]
        public async Task GetByIdOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };
            var order = await orderRepository.SaveAsync(OrderHelper.CreateOrder(cartLines, userId: user.Id));

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.GetByIdAccess);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.GetById(order.Id));
            var resultOrder = objectResult!.Value as Order;
            Assert.IsNotNull(resultOrder);
            Assert.AreEqual(resultOrder!.Id, order.Id);
            Assert.AreEqual(resultOrder!.UserId, user.Id);
            Assert.True(resultOrder!.OrderItems.Any());
            var orderItems = resultOrder!.OrderItems.ToList();
            for (var i = 0; i < orderItems.Count(); i++)
            {
                Assert.AreEqual(cartLines[i].ProductId, orderItems[i].ProductId);
                Assert.AreEqual(cartLines[i].Quantity, orderItems[i].Quantity);
            }
        }

        [Test]
        public async Task GetByIdNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotAllowedRequest(async () => await orderController.GetById(Guid.NewGuid()));
        }

        [Test]
        public async Task GetByIdNotFound()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.GetByIdAccess);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotFoundRequest(async () => await orderController.GetById(Guid.NewGuid()));
        }

        [Test]
        public async Task GetMyOrdersOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };
            var order = await orderRepository.SaveAsync(OrderHelper.CreateOrder(cartLines, userId: user.Id));

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.GetMyOrderAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.GetMyOrders(user.Id));
            var resultOrders = objectResult!.Value as IEnumerable<Order>;
            Assert.IsNotNull(resultOrders);
            Assert.True(resultOrders!.Any());
        }

        [Test]
        public async Task GetMyOrdersNotAllowedAccess()
        {
            var userId = Guid.NewGuid();
            var orderController = GetOrderController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None, userId: userId);
            ControllerHelper.SetControllerContext(orderController, claims);
            await RequestHelper.NotAllowedRequest(async () => await orderController.GetMyOrders(userId));
        }

        [Test]
        public async Task GetMyOrdersNotAllowedForeign()
        {
            var orderController = GetOrderController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.GetMyOrderAccess, userId: Guid.NewGuid());
            ControllerHelper.SetControllerContext(orderController, claims);
            await RequestHelper.NotAllowedRequest(async () => await orderController.GetMyOrders(userId: Guid.NewGuid()));
        }

        [Test]
        public async Task CreateOrderOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.CreateOrderAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(orderController, claims);

            var address = AddressHelper.CreateAddress();
            var contactData = ContactDataHelper.CreateContactData();

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.CreateOrder(user.Id, address, contactData));
            var resultOrder = objectResult!.Value as Order;
            Assert.IsNotNull(resultOrder);
            Assert.AreEqual(resultOrder!.UserId, user.Id);
            Assert.True(resultOrder!.OrderItems.Any());
            var orderItems = resultOrder!.OrderItems.ToList();
            for (var i = 0; i < orderItems.Count(); i++)
            {
                Assert.AreEqual(cartLines[i].ProductId, orderItems[i].ProductId);
                Assert.AreEqual(cartLines[i].Quantity, orderItems[i].Quantity);
            }
        }

        [Test]
        public async Task CreateOrderNotAllowedAccess()
        {
            var userId = Guid.NewGuid();
            var orderController = GetOrderController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None, userId: userId);
            ControllerHelper.SetControllerContext(orderController, claims);
            var address = AddressHelper.CreateAddress();
            var contactData = ContactDataHelper.CreateContactData();
            await RequestHelper.NotAllowedRequest(async () => await orderController.CreateOrder(userId, address, contactData));
        }

        [Test]
        public async Task CreateOrderNotAllowedForeign()
        {
            var orderController = GetOrderController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.CreateOrderAccess, userId: Guid.NewGuid());
            ControllerHelper.SetControllerContext(orderController, claims);
            var address = AddressHelper.CreateAddress();
            var contactData = ContactDataHelper.CreateContactData();
            await RequestHelper.NotAllowedRequest(async () => await orderController.CreateOrder(userId: Guid.NewGuid(), address, contactData));
        }

        [Test]
        public async Task CancelOrderOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };
            var order = await orderRepository.SaveAsync(OrderHelper.CreateOrder(cartLines, userId: user.Id));

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.CancelOrderAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.CancelOrder(order.Id));
            Assert.IsNotNull(objectResult.Value);
        }

        [Test]
        public async Task CancelOrderNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotAllowedRequest(async () => await orderController.CancelOrder(Guid.NewGuid()));
        }

        [Test]
        public async Task SendOrderOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };
            var order = OrderHelper.CreateOrder(cartLines, userId: user.Id);
            order.ChangeStatus(OrderStatus.Handled);
            var savedOrder = await orderRepository.SaveAsync(order);

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.SendOrderAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.CancelOrder(order.Id));
            Assert.IsNotNull(objectResult.Value);
        }

        [Test]
        public async Task SendOrderNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotAllowedRequest(async () => await orderController.SendOrder(Guid.NewGuid()));
        }

        [Test]
        public async Task ConfirmOrderOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };
            var order = OrderHelper.CreateOrder(cartLines, userId: user.Id);
            var savedOrder = await orderRepository.SaveAsync(order);

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.SendOrderAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.ConfirmOrder(order.Id));
            Assert.IsNotNull(objectResult.Value);
        }

        [Test]
        public async Task ConfirmOrderNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotAllowedRequest(async () => await orderController.ConfirmOrder(Guid.NewGuid()));
        }

        [Test]
        public async Task DeliveOrderOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };
            var order = OrderHelper.CreateOrder(cartLines, userId: user.Id);
            order.ChangeStatus(OrderStatus.Handled);
            order.ChangeStatus(OrderStatus.Sent);
            var savedOrder = await orderRepository.SaveAsync(order);

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.SendOrderAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.DeliveOrder(order.Id));
            Assert.IsNotNull(objectResult.Value);
        }

        [Test]
        public async Task DeliveOrderNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotAllowedRequest(async () => await orderController.DeliveOrder(Guid.NewGuid()));
        }

        [Test]
        public async Task ReceiveOrderOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var productRepository = new DbProductRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);

            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product1 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct1"));
            var product2 = await productRepository.SaveAsync(ProductHelper.CreateProduct(name: "testProduct2"));
            var cartLines = new List<CartLine>
            {
                await cartLineRepository.SaveAsync(new(user.Id, product1.Id, 15)),
                await cartLineRepository.SaveAsync(new(user.Id, product2.Id, 25)),
            };
            var order = OrderHelper.CreateOrder(cartLines, userId: user.Id);
            order.ChangeStatus(OrderStatus.Handled);
            order.ChangeStatus(OrderStatus.Sent);
            order.ChangeStatus(OrderStatus.Delivered);
            var savedOrder = await orderRepository.SaveAsync(order);

            var orderController = GetOrderController(userRepository: userRepository, productRepository: productRepository,
                orderRepository: orderRepository, cartLineRepository: cartLineRepository);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: OrderController.SendOrderAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(orderController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await orderController.ReceiveOrder(order.Id));
            Assert.IsNotNull(objectResult.Value);
        }

        [Test]
        public async Task ReceiveOrderNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var orderController = GetOrderController();
            ControllerHelper.SetControllerContext(orderController, user);
            await RequestHelper.NotAllowedRequest(async () => await orderController.ReceiveOrder(Guid.NewGuid()));
        }

        private OrderController GetOrderController(IOrderRepository? orderRepository = null, IUserRepository? userRepository = null,
            ICartLineRepository? cartLineRepository = null, IProductRepository? productRepository = null,
            SecurityService? securityService = null, Cart? cart = null, PriceCalculator? priceCalculator = null, SaleService? saleService = null)
        {
            orderRepository ??= new DbOrderRepository(_dbContext!);
            userRepository ??= new DbUserRepository(_dbContext!);
            productRepository ??= new DbProductRepository(_dbContext!);
            cartLineRepository ??= new DbCartLineRepository(_dbContext!);
            securityService ??= new SecurityService(userRepository);
            cart ??= new Cart(cartLineRepository);
            priceCalculator ??= new PriceCalculator(productRepository);
            saleService ??= new SaleService(priceCalculator, cart, orderRepository, productRepository, userRepository);

            return new OrderController(cart, saleService, securityService, orderRepository, userRepository);
        }
    }
}
