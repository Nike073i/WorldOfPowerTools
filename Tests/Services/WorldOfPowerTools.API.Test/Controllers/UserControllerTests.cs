using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorldOfPowerTools.API.Controllers;
using WorldOfPowerTools.API.RequestModels.User;
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
    public class UserControllerTests : ControllerBaseTests
    {
        [Test]
        public async Task GetAllOk()
        {
            await InitializeData();

            var userController = GetUserController();

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: UserController.GetAllAccess);
            ControllerHelper.SetControllerContext(userController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await userController.GetAll());
            var cartLines = objectResult!.Value as IEnumerable<User>;
            Assert.True(cartLines!.Any());
        }

        [Test]
        public async Task GetAllNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            await RequestHelper.NotAllowedRequest(async () => await userController.GetAll());
        }

        [Test]
        public async Task GetByIdOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var userController = GetUserController(userRepository: userRepository);
            var user = await userRepository.SaveAsync(UserHelper.CreateUser());

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: UserController.GetByIdAccess);
            ControllerHelper.SetControllerContext(userController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await userController.GetById(user.Id));
            var resultUser = objectResult!.Value as User;
            Assert.NotNull(resultUser);
            Assert.AreEqual(user.Login, resultUser!.Login);
            Assert.AreEqual(user.Id, resultUser!.Id);
            Assert.AreEqual(user.Address, resultUser!.Address);
            Assert.AreEqual(user.ContactData, resultUser!.ContactData);
            Assert.AreEqual(user.PasswordHash, resultUser!.PasswordHash);
            Assert.AreEqual(user.Rights, resultUser!.Rights);
            Assert.AreEqual(user.PersonData, resultUser!.PersonData);
        }

        [Test]
        public async Task GetByIdNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            await RequestHelper.NotAllowedRequest(async () => await userController.GetById(Guid.NewGuid()));
        }

        [Test]
        public async Task GetByIdNotFound()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.Users);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            await RequestHelper.NotFoundRequest(async () => await userController.GetById(Guid.NewGuid()));
        }

        [Test]
        public async Task RemoveUserOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var userController = GetUserController(userRepository: userRepository);
            var user = await userRepository.SaveAsync(UserHelper.CreateUser());

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: UserController.RemoveAccess);
            ControllerHelper.SetControllerContext(userController, claims);

            var objectResult = await RequestHelper.OkRequest(async () => await userController.RemoveUser(user.Id));
            var resultUserId = (Guid)objectResult.Value!;
            Assert.NotNull(resultUserId);
            Assert.AreEqual(user.Id, resultUserId);
        }

        [Test]
        public async Task RemoveUserNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            await RequestHelper.NotAllowedRequest(async () => await userController.RemoveUser(Guid.NewGuid()));
        }

        [Test]
        public async Task AddUserRightsOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var userController = GetUserController(userRepository: userRepository);
            var user = await userRepository.SaveAsync(UserHelper.CreateUser(userRights: Actions.None));

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: UserController.AddUserRightsAccess);
            ControllerHelper.SetControllerContext(userController, claims);

            var newRights = Actions.Products | Actions.Cart;
            var model = new ChangeUserRightModel
            {
                UserId = user.Id,
                Action = newRights
            };
            var objectResult = await RequestHelper.OkRequest(async () => await userController.AddUserRights(model));
            var resultUser = objectResult.Value as User;

            Assert.NotNull(resultUser);
            Assert.AreEqual(user.Id, resultUser!.Id);
            Assert.AreEqual(user.Rights, newRights);
        }

        [Test]
        public async Task AddUserRightsNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            var model = new ChangeUserRightModel
            {
                UserId = Guid.NewGuid(),
                Action = Actions.Users
            };
            await RequestHelper.NotAllowedRequest(async () => await userController.AddUserRights(model));
        }

        [Test]
        public async Task AddUserRightsNotFound()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.Users);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            var model = new ChangeUserRightModel
            {
                UserId = Guid.NewGuid(),
                Action = Actions.Users
            };
            await RequestHelper.NotFoundRequest(async () => await userController.AddUserRights(model));
        }

        [Test]
        public async Task RemoveUserRightsOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            var userController = GetUserController(userRepository: userRepository);
            var user = await userRepository.SaveAsync(UserHelper.CreateUser(userRights: Actions.Products | Actions.Cart));

            var claims = ClaimsPrincipalHelper.CreateUser(userRights: UserController.RemoveUserRightsAccess);
            ControllerHelper.SetControllerContext(userController, claims);

            var removeRights = Actions.Products | Actions.Cart;
            var model = new ChangeUserRightModel
            {
                UserId = user.Id,
                Action = removeRights
            };
            var objectResult = await RequestHelper.OkRequest(async () => await userController.RemoveUserRights(model));
            var resultUser = objectResult.Value as User;

            Assert.NotNull(resultUser);
            Assert.AreEqual(user.Id, resultUser!.Id);
            Assert.AreEqual(user.Rights, Actions.None);
        }

        [Test]
        public async Task RemoveUserRightsNotAllowed()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.None);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            var model = new ChangeUserRightModel
            {
                UserId = Guid.NewGuid(),
                Action = Actions.Users
            };
            await RequestHelper.NotAllowedRequest(async () => await userController.RemoveUserRights(model));
        }

        [Test]
        public async Task RemoveUserRightsNotFound()
        {
            var user = ClaimsPrincipalHelper.CreateUser(userRights: Actions.Users);
            var userController = GetUserController();
            ControllerHelper.SetControllerContext(userController, user);
            var model = new ChangeUserRightModel
            {
                UserId = Guid.NewGuid(),
                Action = Actions.Users
            };
            await RequestHelper.NotFoundRequest(async () => await userController.RemoveUserRights(model));
        }

        private UserController GetUserController(IUserRepository? userRepository = null, SecurityService? securityService = null)
        {
            userRepository ??= new DbUserRepository(_dbContext!);
            securityService ??= new SecurityService(userRepository);

            return new UserController(securityService, userRepository);
        }
    }
}
