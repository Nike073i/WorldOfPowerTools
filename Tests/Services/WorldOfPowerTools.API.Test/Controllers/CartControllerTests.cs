using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldOfPowerTools.API.Controllers;
using WorldOfPowerTools.API.RequestModels.Cart;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Data;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Authorization;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Controllers;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Requests;
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.ObjectValues;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Test.Controllers
{
    public class CartControllerTests : ControllerBaseTests
    {
        [Test]
        public async Task GetProductsOk()
        {
            await InitializeData();

            var userRepository = new DbUserRepository(_dbContext!);

            var cartController = GetCartController(userRepository: userRepository);
            var user = await userRepository.GetByLoginAsync(DbInitializer.user1Login);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.GetProductsAccess, userId: user!.Id);
            ControllerHelper.SetControllerContext(cartController, claims);


            var objectResult = await RequestHelper.OkRequest(async () => await cartController.GetProducts(user!.Id));
            var cartLines = objectResult!.Value as IEnumerable<CartLine>;
            Assert.True(cartLines!.Any());
        }

        [Test]
        public async Task GetProductsNotAllowedAccess()
        {
            var userId = Guid.NewGuid();
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None, userId: userId);
            ControllerHelper.SetControllerContext(cartController, claims);
            await RequestHelper.NotAllowedRequest(async () => await cartController.GetProducts(userId));
        }

        [Test]
        public async Task GetProductsNotAllowedForeign()
        {
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.GetProductsAccess, userId: Guid.NewGuid());
            ControllerHelper.SetControllerContext(cartController, claims);
            await RequestHelper.NotAllowedRequest(async () => await cartController.GetProducts(userId: Guid.NewGuid()));
        }

        [Test]
        public async Task AddProductOk()
        {
            var productRepository = new DbProductRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);
            var userRepository = new DbUserRepository(_dbContext!);

            var cartController = GetCartController(cartLineRepository: cartLineRepository, userRepository: userRepository);
            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct());
            var quantity = 20;

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.AddProductAccess, userId: user!.Id);
            ControllerHelper.SetControllerContext(cartController, claims);
            var model = new AddProductToCartModel
            {
                UserId = user.Id,
                ProductId = product.Id,
                Quantity = quantity
            };
            var objectResult = await RequestHelper.OkRequest(async () => await cartController.AddProduct(model));
            var cartLines = await cartLineRepository.GetByUserIdAsync(user.Id);
            Assert.True(cartLines.Any());
            var cartLine = cartLines.First(cl => cl.ProductId == product.Id);
            Assert.AreEqual(cartLine.Quantity, quantity);
        }

        [Test]
        public async Task AddProductNotAllowedAccess()
        {
            var userId = Guid.NewGuid();
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None, userId: userId);
            ControllerHelper.SetControllerContext(cartController, claims);
            var model = new AddProductToCartModel
            {
                UserId = userId,
                ProductId = Guid.NewGuid(),
                Quantity = 150
            };
            await RequestHelper.NotAllowedRequest(async () => await cartController.AddProduct(model));
        }

        [Test]
        public async Task AddProductNotAllowedForeign()
        {
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.AddProductAccess, userId: Guid.NewGuid());
            ControllerHelper.SetControllerContext(cartController, claims);
            var model = new AddProductToCartModel
            {
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 150
            };
            await RequestHelper.NotAllowedRequest(async () => await cartController.AddProduct(model));
        }

        [Test]
        public async Task RemoveProductOk()
        {
            var productRepository = new DbProductRepository(_dbContext!);
            var cartLineRepository = new DbCartLineRepository(_dbContext!);
            var userRepository = new DbUserRepository(_dbContext!);

            var cartController = GetCartController(cartLineRepository: cartLineRepository, userRepository: userRepository);
            var user = await userRepository.SaveAsync(UserHelper.CreateUser());
            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct());
            var quantity = 20;

            await cartLineRepository.SaveAsync(new(user.Id, product.Id, quantity));

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.RemoveProductAccess, userId: user.Id);
            ControllerHelper.SetControllerContext(cartController, claims);

            var model = new RemoveProductModel
            {
                UserId = user.Id,
                ProductId = product.Id
            };
            var objectResult = await RequestHelper.OkRequest(async () => await cartController.RemoveProduct(model));
            var cartLines = await cartLineRepository.GetByUserIdAsync(user.Id);
            Assert.True(!cartLines.Any());
        }

        [Test]
        public async Task RemoveProductNotAllowedAccess()
        {
            var userId = Guid.NewGuid();
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None, userId: userId);
            ControllerHelper.SetControllerContext(cartController, claims);
            var model = new RemoveProductModel
            {
                UserId = userId,
                ProductId = Guid.NewGuid()
            };
            await RequestHelper.NotAllowedRequest(async () => await cartController.RemoveProduct(model));
        }

        [Test]
        public async Task RemoveProductNotAllowedForeign()
        {
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.RemoveProductAccess, userId: Guid.NewGuid());
            ControllerHelper.SetControllerContext(cartController, claims);
            var model = new RemoveProductModel
            {
                UserId = Guid.NewGuid(),
                ProductId = Guid.NewGuid()
            };
            await RequestHelper.NotAllowedRequest(async () => await cartController.RemoveProduct(model));
        }

        [Test]
        public async Task ClearCartOk()
        {
            await InitializeData();
            var cartLineRepository = new DbCartLineRepository(_dbContext!);
            var userRepository = new DbUserRepository(_dbContext!);
            var cartController = GetCartController(cartLineRepository: cartLineRepository, userRepository: userRepository);

            var user = await userRepository.GetByLoginAsync(DbInitializer.user1Login);

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.ClearCartAccess, userId: user!.Id);
            ControllerHelper.SetControllerContext(cartController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await cartController.ClearCart(user.Id));
            var cartLines = await cartLineRepository.GetByUserIdAsync(user.Id);
            Assert.False(cartLines.Any());
        }

        [Test]
        public async Task ClearCartNotAllowedAccess()
        {
            var userId = Guid.NewGuid();
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None, userId: userId);
            ControllerHelper.SetControllerContext(cartController, claims);
            await RequestHelper.NotAllowedRequest(async () => await cartController.ClearCart(userId));
        }

        [Test]
        public async Task ClearCartNotAllowedForeign()
        {
            var cartController = GetCartController();
            var claims = ClaimsPrincipalHelper.CreateUser(userRights: CartController.ClearCartAccess, userId: Guid.NewGuid());
            ControllerHelper.SetControllerContext(cartController, claims);
            await RequestHelper.NotAllowedRequest(async () => await cartController.ClearCart(Guid.NewGuid()));
        }

        private CartController GetCartController(IUserRepository? userRepository = null,
            ICartLineRepository? cartLineRepository = null, SecurityService? securityService = null, Cart? cart = null)
        {
            userRepository ??= new DbUserRepository(_dbContext!);
            cartLineRepository ??= new DbCartLineRepository(_dbContext!);
            securityService ??= new SecurityService(userRepository);
            cart ??= new Cart(cartLineRepository);

            return new CartController(securityService, cart);
        }
    }
}
