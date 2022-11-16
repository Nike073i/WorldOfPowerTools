using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldOfPowerTools.API.Controllers;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Data;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Authorization;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Controllers;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Requests;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Test.Controllers
{
    public class ProductControllerTests
    {
        private WorldOfPowerToolsDb? _dbContext;

        [SetUp]
        public void SetUp()
        {
            var dbContextHelper = new DbContextHelper();
            _dbContext = dbContextHelper.DbContext;
        }

        private async Task InitializeData()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            await dbInitializer!.InitializeAsync();
        }

        [Test]
        public async Task GetAllProduct()
        {
            await InitializeData();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);

            var response = await productController.GetAll();
            Assert.IsNotNull(response);

            var objectResult = response as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.True(objectResult!.StatusCode == StatusCodes.Status200OK);

            var products = objectResult!.Value as IEnumerable<Product>;
            Assert.True(products!.Any());
        }

        [Test]
        public async Task GetByCategory() { }

        [Test]
        public async Task GetByIdOk() { }

        [Test]
        public async Task GetByIdNotFound() { }

        [Test]
        public async Task AddProductOk() { }

        [Test]
        public async Task AddProductNotAllowed() { }

        [Test]
        public async Task RemoveProductOk() { }

        [Test]
        public async Task RemoveProductNotAllowed() { }

        [Test]
        public async Task LoadingProductOk()
        {
            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.Products);
            ControllerHelper.SetControllerContext(productController, user);

            var productQuantity = 50;
            var productAddition = 15;
            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct(quantity: productQuantity));

            var response = await productController.AddProductToStore(product.Id, 15);
            Assert.IsNotNull(response);

            var objectResult = response as ObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.True(objectResult!.StatusCode == StatusCodes.Status200OK);

            var resultProduct = objectResult!.Value as Product;
            Assert.IsNotNull(resultProduct);
            Assert.AreEqual(resultProduct!.Quantity, productQuantity + productAddition);
        }

        [Test]
        public async Task LoadingProductNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = ProductHelper.TestProductId;
            await RequestHelper.NotAllowedRequest(async () => await productController.AddProductToStore(productId, 15));
        }

        [Test]
        public async Task LoadingProductNotFound()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.Products);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = Guid.NewGuid();
            await RequestHelper.NotFoundRequest(async () => await productController.AddProductToStore(productId, 15));
        }

        [Test]
        public async Task UnloadingProductOk() { }

        [Test]
        public async Task UnloadingProductNotAllowed() { }

        [Test]
        public async Task UnloadingProductNotFound() { }

        [Test]
        public async Task UpdateProductOk() { }

        [Test]
        public async Task UpdateProductNotAllowed() { }

        [Test]
        public async Task UpdateProductNotFound() { }

        private ProductController GetProductController(IProductRepository? productRepository = null, IUserRepository? userRepository = null,
            IOrderRepository? orderRepository = null, SecurityService? securityService = null)
        {
            productRepository ??= new DbProductRepository(_dbContext!);
            userRepository ??= new DbUserRepository(_dbContext!);
            orderRepository ??= new DbOrderRepository(_dbContext!);
            securityService ??= new SecurityService(userRepository);

            return new ProductController(securityService, productRepository, orderRepository);
        }
    }
}
