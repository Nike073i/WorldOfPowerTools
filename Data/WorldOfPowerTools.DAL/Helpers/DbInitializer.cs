using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.DAL.Helpers
{
    public class DbInitializer
    {
        private readonly WorldOfPowerToolsDb _dbContext;

        public static readonly string user1Login = "user1";
        public static readonly string user2Login = "user2";
        public static readonly string user3Login = "user3";
        public static readonly string user4Login = "user4";
        public static readonly string user5Login = "user5";

        public DbInitializer(WorldOfPowerToolsDb dbContext)
        {
            _dbContext = dbContext;
        }

        public void RecreateDatabase()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Database.EnsureCreated();
        }

        public async Task InitializeAsync()
        {
            await InsertProductsAsync();
            await InsertUsersAsync();
            await InsertOrdersAsync();
            await InsertCartLinesAsync();
        }

        public async Task InsertProductsAsync()
        {
            var productRepository = new DbProductRepository(_dbContext);

            var products = new List<Product>
            {
                new("product1", 100, Category.Jigsaw, "description1", 100),
                new("product2", 200, Category.Caulkgun, "description2", 200),
                new("product3", 300, Category.Jigsaw, "description3", 300),
                new("product4", 400, Category.Perforator, "description4", 400),
                new("product5", 500, Category.Perforator, "description5", 500),
            };
            foreach (var product in products)
            {
                await productRepository.SaveAsync(product);
            }
        }

        public async Task InsertUsersAsync()
        {
            var userRepository = new DbUserRepository(_dbContext);

            var users = new List<User>
            {
                new(user1Login,"7c6a180b36896a0a8c02787eeafb0e4c", Actions.MyOrders | Actions.Cart),
                new(user2Login,"6cb75f652a9b52798eb6cf2201057c73", Actions.MyOrders | Actions.Cart),
                new(user3Login,"819b0643d6b89dc9b579fdfc9094f28e", Actions.MyOrders | Actions.Cart),
                new(user4Login,"34cc93ece0ba9e3f6f235d4af979b16c", Actions.Products | Actions.Cart | Actions.MyOrders | Actions.AllOrders),
                new(user5Login,"db0edd04aaac4506f7edab03ac855d56", Actions.Products | Actions.Cart | Actions.MyOrders | Actions.Users | Actions.AllOrders),
            };

            foreach (var user in users)
            {
                await userRepository.SaveAsync(user);
            }
        }

        public async Task InsertOrdersAsync()
        {
            var productRepository = new DbProductRepository(_dbContext);
            var orderRepository = new DbOrderRepository(_dbContext);
            var userRepository = new DbUserRepository(_dbContext);

            var users = userRepository.GetAllAsync().Result.ToList();
            var products = productRepository.GetAllAsync().Result.ToList();

            var orders = new List<Order>
            {
                new(users[0].Id, 1500,
                    new("country1", "city1", "house1", "flat1", 15, "111111"), new("+79176316458", "kit1073i@miif.ry"),
                    new List<CartLine>{
                        new(users[0].Id, products[0].Id, 15),
                        new(users[0].Id, products[2].Id, 25),
                        new(users[0].Id, products[3].Id, 35),
                    }),
                new(users[1].Id, 2500,
                    new("country2", "city2", "house2", "flat2", 25, "222222"), new("+79276316458", "kit2073i@miif.ry"),
                    new List<CartLine>{
                        new(users[1].Id, products[1].Id, 15),
                        new(users[1].Id, products[3].Id, 25),
                        new(users[1].Id, products[4].Id, 35),
                    }),
                 new(users[3].Id, 4500,
                    new("country4", "city4", "house4", "flat4", 45, "444444"), new("+79476316458", "kit4073i@miif.ry"),
                    new List<CartLine>{
                        new(users[3].Id, products[0].Id, 15),
                        new(users[3].Id, products[2].Id, 25),
                    }),
            };

            foreach (var order in orders)
            {
                await orderRepository.SaveAsync(order);
            }
        }

        public async Task InsertCartLinesAsync()
        {
            var productRepository = new DbProductRepository(_dbContext);
            var userRepository = new DbUserRepository(_dbContext);
            var cartLineRepository = new DbCartLineRepository(_dbContext);

            var users = userRepository.GetAllAsync().Result.ToList();
            var products = productRepository.GetAllAsync().Result.ToList();

            var cartLines = new List<CartLine>
            {
                new(users[0].Id, products[0].Id, 15),
                new(users[0].Id, products[1].Id, 25),
                new(users[0].Id, products[2].Id, 35),
                new(users[1].Id, products[0].Id, 5),
                new(users[1].Id, products[2].Id, 7),
                new(users[1].Id, products[4].Id, 8),
                new(users[2].Id, products[0].Id, 4),
                new(users[2].Id, products[1].Id, 9),
                new(users[3].Id, products[2].Id, 3),
                new(users[4].Id, products[0].Id, 10),
            };

            foreach (var cartLine in cartLines)
            {
                await cartLineRepository.SaveAsync(cartLine);
            }
        }
    }
}
