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
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Test.Controllers
{
    public class ProductControllerTests : ControllerBaseTests
    {
        [Test]
        public async Task GetAllProduct()
        {
            await InitializeData();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);

            var objectResult = await RequestHelper.OkRequest(async () => await productController.GetAll());
            var products = objectResult!.Value as IEnumerable<Product>;
            Assert.True(products!.Any());
        }

        [Test]
        public async Task GetByCategory()
        {
            await InitializeData();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);

            var objectResult = await RequestHelper.OkRequest(async () => await productController.GetByCategory(Category.Perforator));
            var products = objectResult!.Value as IEnumerable<Product>;
            Assert.True(products!.Any());
        }

        [Test]
        public async Task GetByIdOk()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            dbInitializer.RecreateDatabase();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);

            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct());

            var objectResult = await RequestHelper.OkRequest(async () => await productController.GetById(product.Id));
            var resultProduct = objectResult.Value as Product;
            Assert.IsNotNull(resultProduct);
        }

        [Test]
        public async Task GetByIdNotFound()
        {
            var productController = GetProductController();
            var productId = Guid.NewGuid();
            await RequestHelper.NotFoundRequest(async () => await productController.GetById(productId));
        }

        [Test]
        public async Task AddProductOk()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            dbInitializer.RecreateDatabase();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.AddProductAccess);
            ControllerHelper.SetControllerContext(productController, user);

            var productName = "newName";
            var productPrice = 500;
            var productDescription = "newDescription";
            var productQuantity = 100;

            var objectResult = await RequestHelper.OkRequest(async () => await productController.AddProduct(productName, productPrice, productDescription, productQuantity));
            var resultProduct = objectResult!.Value as Product;
            Assert.IsNotNull(resultProduct);
            Assert.True(resultProduct!.Id != Guid.Empty);
            Assert.AreEqual(resultProduct.Name, productName);
            Assert.AreEqual(resultProduct.Price, productPrice);
            Assert.AreEqual(resultProduct.Description, productDescription);
            Assert.AreEqual(resultProduct.Quantity, productQuantity);
        }

        [Test]
        public async Task AddProductNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);

            var productName = "newName";
            var productPrice = 500;
            var productDescription = "newDescription";
            var productQuantity = 100;

            await RequestHelper.NotAllowedRequest(async () => await productController.AddProduct(productName, productPrice, productDescription, productQuantity));
        }

        [Test]
        public async Task RemoveProductOk()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            dbInitializer.RecreateDatabase();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.RemoveProductAccess);
            ControllerHelper.SetControllerContext(productController, user);

            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct());

            var objectResult = await RequestHelper.OkRequest(async () => await productController.RemoveProduct(product.Id));
            var resultProductId = (Guid)objectResult.Value!;
            Assert.IsNotNull(resultProductId);
            Assert.AreEqual(resultProductId, product.Id);
        }

        [Test]
        public async Task RemoveProductNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = ProductHelper.TestProductId;
            await RequestHelper.NotAllowedRequest(async () => await productController.RemoveProduct(productId));
        }

        [Test]
        public async Task LoadingProductOk()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            dbInitializer.RecreateDatabase();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.LoadingProductAccess);
            ControllerHelper.SetControllerContext(productController, user);

            var productQuantity = 50;
            var productAddition = 15;
            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct(quantity: productQuantity));

            var objectResult = await RequestHelper.OkRequest(async () => await productController.AddProductToStore(product.Id, productAddition));
            var resultProduct = objectResult.Value as Product;
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
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.LoadingProductAccess);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = Guid.NewGuid();
            await RequestHelper.NotFoundRequest(async () => await productController.AddProductToStore(productId, 15));
        }

        [Test]
        public async Task UnloadingProductOk()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            dbInitializer.RecreateDatabase();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.LoadingProductAccess);
            ControllerHelper.SetControllerContext(productController, user);

            var productQuantity = 50;
            var productRemoval = 15;
            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct(quantity: productQuantity));

            var objectResult = await RequestHelper.OkRequest(async () => await productController.RemoveProductFromStore(product.Id, productRemoval));
            var resultProduct = objectResult.Value as Product;
            Assert.IsNotNull(resultProduct);
            Assert.AreEqual(resultProduct!.Quantity, productQuantity - productRemoval);
        }

        [Test]
        public async Task UnloadingProductNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = ProductHelper.TestProductId;
            await RequestHelper.NotAllowedRequest(async () => await productController.RemoveProductFromStore(productId, 15));
        }

        [Test]
        public async Task UnloadingProductNotFound()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.UnloadingProductAccess);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = Guid.NewGuid();
            await RequestHelper.NotFoundRequest(async () => await productController.RemoveProductFromStore(productId, 15));
        }

        [Test]
        public async Task UpdateProductOk()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            dbInitializer.RecreateDatabase();

            var productRepository = new DbProductRepository(_dbContext!);
            var productController = GetProductController(productRepository: productRepository);
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.UpdateProductAccess);
            ControllerHelper.SetControllerContext(productController, user);

            var product = await productRepository.SaveAsync(ProductHelper.CreateProduct());
            var productNewName = "newName";
            var productNewPrice = 500;
            var productNewDescription = "newDescription";
            var productNewCategory = Category.Caulkgun;

            var objectResult = await RequestHelper.OkRequest(async () => await productController.UpdateProduct(
                productId: product.Id,
                name: productNewName,
                price: productNewPrice,
                description: productNewDescription,
                category: productNewCategory
                ));
            var resultProduct = objectResult.Value as Product;
            Assert.IsNotNull(resultProduct);
            Assert.AreEqual(resultProduct!.Id, product.Id);
            Assert.AreEqual(resultProduct!.Name, productNewName);
            Assert.AreEqual(resultProduct!.Price, productNewPrice);
            Assert.AreEqual(resultProduct!.Description, productNewDescription);
            Assert.AreEqual(resultProduct!.Category, productNewCategory);
        }

        [Test]
        public async Task UpdateProductNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = ProductHelper.TestProductId;
            await RequestHelper.NotAllowedRequest(async () => await productController.UpdateProduct(productId, name: "nameNotFound"));
        }

        [Test]
        public async Task UpdateProductNotFound()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: ProductController.UpdateProductAccess);
            var productController = GetProductController();
            ControllerHelper.SetControllerContext(productController, user);
            var productId = Guid.NewGuid();
            await RequestHelper.NotFoundRequest(async () => await productController.UpdateProduct(productId, name: "nameNotFound"));
        }

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
