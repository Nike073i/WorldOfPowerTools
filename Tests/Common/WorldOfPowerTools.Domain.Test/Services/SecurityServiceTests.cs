using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.Domain.Test.Services
{
    public class SecurityServiceTests
    {
        [Test]
        public async Task UserOperationAvailabilityNullUserId()
        {
            var userRepository = new Mock<IUserRepository>().Object;
            var service = new SecurityService(userRepository);
            var actions = Actions.Products;
            var access = await service.UserOperationAvailability(Guid.Empty, actions);
            Assert.False(access);
        }

        [Test]
        [TestCase(Actions.None, Actions.Cart, false)]
        [TestCase(Actions.Cart | Actions.Users, Actions.None, true)]
        [TestCase(Actions.Cart, Actions.Cart, true)]
        [TestCase(Actions.Cart | Actions.MyOrders, Actions.Cart, true)]
        [TestCase(Actions.Cart | Actions.MyOrders, Actions.Users, false)]
        [TestCase(Actions.Cart | Actions.MyOrders, Actions.Products, false)]
        [TestCase(Actions.Cart | Actions.MyOrders | Actions.Products, Actions.Products, true)]
        public async Task UserOperationAvailabilityCorrect(Actions userRights, Actions action, bool awaitingAccess)
        {
            var userId = new Guid("f1463d18-7eed-4347-999c-cb96992570b4");
            var user = new User("login", "7c6a180b36896a0a8c02787eeafb0e4c", userRights);

            var mock = new Mock<IUserRepository>();
            mock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            var service = new SecurityService(mock.Object);
            var access = await service.UserOperationAvailability(userId, action);
            Assert.AreEqual(access, awaitingAccess);
        }
    }
}
