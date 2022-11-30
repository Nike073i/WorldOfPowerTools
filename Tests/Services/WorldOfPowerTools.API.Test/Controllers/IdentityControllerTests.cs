using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System.Threading.Tasks;
using WorldOfPowerTools.API.Controllers;
using WorldOfPowerTools.API.RequestModels.Identity;
using WorldOfPowerTools.API.Services;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Requests;
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Test.Controllers
{
    public class IdentityControllerTests : ControllerBaseTests
    {
        [Test]
        public async Task AuthorizationOk()
        {
            var userRepository = new DbUserRepository(_dbContext!);
            await userRepository.SaveAsync(new("userName", "221068207e125b97beb4e2d062e888b1", Domain.Enums.Actions.Cart));

            var identityController = GetIdentityController(userRepository: userRepository);
            var model = new IdentityModel
            {
                Login = "userName",
                Password = "userPassword"
            };
            var objectResult = await RequestHelper.OkRequest(async () => await identityController.Authorization(model));
            var jwt = objectResult.Value as string;
            Assert.IsNotNull(jwt);
        }

        [Test]
        public async Task AuthorizationNotFound()
        {
            var identityController = GetIdentityController();
            var model = new IdentityModel
            {
                Login = "userName",
                Password = "userPassword"
            };
            await RequestHelper.NotFoundRequest(async () => await identityController.Authorization(model));
        }

        [Test]
        public async Task Registration()
        {
            var identityController = GetIdentityController();
            var userlogin = "newUserLogin";
            var userPassword = "newUserPassword";
            var model = new IdentityModel
            {
                Login = userlogin,
                Password = userPassword
            };
            var objectResult = await RequestHelper.OkRequest(async () => await identityController.Registration(model));
            var resultUser = objectResult.Value as User;
            Assert.IsNotNull(resultUser);
            Assert.AreEqual(resultUser!.Login, userlogin);
        }

        private IdentityController GetIdentityController(IUserRepository? userRepository = null, IdentityService? identityService = null, JwtService? jwtService = null)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            userRepository ??= new DbUserRepository(_dbContext!);
            identityService ??= new IdentityService(userRepository);
            jwtService ??= new JwtService(builder.Build());
            return new IdentityController(identityService, jwtService);
        }
    }
}
