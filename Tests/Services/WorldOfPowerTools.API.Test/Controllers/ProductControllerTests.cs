using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorldOfPowerTools.API.Controllers;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.API.Test.Infrastructure.Authorization;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Data;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities;
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
        public async Task LoadingProduct()
        {
            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);
            var userRights = Actions.Products;
            var user = new TestPrincipal(new Claim(ClaimsPrincipalExtensions.CLAIM_USER_RIGHTS, userRights.ToString()));
            SetControllerContext(productController, user);

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

        private ProductController GetProductController(IProductRepository? productRepository = null)
        {
            productRepository ??= new DbProductRepository(_dbContext!);
            var userRepository = new DbUserRepository(_dbContext!);
            var orderRepository = new DbOrderRepository(_dbContext!);
            var securityService = new SecurityService(userRepository);

            return new ProductController(securityService, productRepository, orderRepository);
        }

        private void SetControllerContext(in ControllerBase controller, ClaimsPrincipal user)
        {
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        }
    }
}
