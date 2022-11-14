using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Exceptions;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;
using WorldOfPowerTools.Domain.Services;
using WorldOfPowerTools.Domain.Test.Models.Entities;

namespace WorldOfPowerTools.Domain.Test.Services
{
    public class IdentityServiceTest
    {
        private static string testUserPassword = "Пароль";
        private static string testUserLogin = "testLogin";
        private static User testUser = new User(testUserLogin, testUserPassword, Actions.Cart | Actions.MyOrders);

        private static User user1 = UserTest.CreateUser(login: "user1", passwordHash: "7c6a180b36896a0a8c02787eeafb0e4c");
        private static User user2 = UserTest.CreateUser(login: "user2", passwordHash: "e96eead43aa05a09e5caddadf513ed3c");
        private static User user3 = UserTest.CreateUser(login: "user3", passwordHash: "819b0643d6b89dc9b579fdfc9094f28e");

        private Guid testUser1Id = new Guid();
        private Guid testUser2Id = new Guid();
        private Guid testUser3Id = new Guid();

        private static string user1password = "password1";
        private static string user2password = "пароль2";

        [Test]
        [TestCase(null, typeof(ArgumentNullException))]
        [TestCase("", typeof(ArgumentNullException))]
        public void PasswordHashIncorrect(string password, Type awaitingException)
        {
            var userRepository = GetUserRepositoryMock();
            var identityService = new IdentityService(userRepository);
            Assert.Throws(awaitingException, () => identityService.PasswordHash(password));
        }

        [Test]
        [TestCaseSource(nameof(UserDataIncorrectCases))]
        public void AuthorizationIncorrect(string login, string password, Type awaitingException)
        {
            var userRepository = GetUserRepositoryMock();
            var identityService = new IdentityService(userRepository);
            Assert.ThrowsAsync(awaitingException, async () => await identityService.Authorization(login, password));
        }

        [Test]
        [TestCaseSource(nameof(AuthorizationCorrectCases))]
        public async Task AuthorizationCorrect(string login, string password, User foundUser)
        {
            var userRepository = GetUserRepositoryMock();
            var identityService = new IdentityService(userRepository);
            var user = await identityService.Authorization(login, password);
            var predicate = user == null ? foundUser == null : UserTest.IsEqualUsers(user, foundUser);
            Assert.IsTrue(predicate);
        }

        [Test]
        [TestCaseSource(nameof(UserDataIncorrectCases))]
        [TestCaseSource(nameof(RegistrationIncorrectCases))]
        public void RegistrationIncorrect(string login, string password, Type awaitingException)
        {
            var userRepository = GetUserRepositoryMock();
            var identityService = new IdentityService(userRepository);
            Assert.ThrowsAsync(awaitingException, async () => await identityService.Registration(login, password));
        }

        [Test]
        public async Task RegistrationCorrect()
        {
            var userRepository = GetUserRepositoryMock();
            var identityService = new IdentityService(userRepository);
            var user = await identityService.Registration(testUserLogin, testUserPassword);
            Assert.NotNull(user);
        }

        private IUserRepository GetUserRepositoryMock()
        {
            var userRepository = new Mock<IUserRepository>();
            userRepository.Setup((x) => x.GetByIdAsync(testUser1Id)).ReturnsAsync(user1);
            userRepository.Setup((x) => x.GetByIdAsync(testUser2Id)).ReturnsAsync(user2);
            userRepository.Setup((x) => x.GetByIdAsync(testUser3Id)).ReturnsAsync(user3);
            userRepository.Setup((x) => x.GetByLoginAsync(user1.Login)).ReturnsAsync(user1);
            userRepository.Setup((x) => x.GetByLoginAsync(user2.Login)).ReturnsAsync(user2);
            userRepository.Setup((x) => x.SaveAsync(It.Is<User>(u => u.Login == testUserLogin))).ReturnsAsync(testUser);
            return userRepository.Object;
        }

        static readonly object[] UserDataIncorrectCases =
        {
            new object?[] { "", testUserPassword,typeof(ArgumentNullException) },
            new object?[] { null, testUserPassword,typeof(ArgumentNullException) },
            new object?[] { testUserLogin, "", typeof(ArgumentNullException) },
            new object?[] { testUserLogin, null, typeof(ArgumentNullException) }
        };

        static readonly object[] RegistrationIncorrectCases =
{
            new object?[] { user1.Login, user1password,typeof(UserExistsException) },
            new object?[] { user2.Login, user2password,typeof(UserExistsException) },
        };

        static readonly object[] AuthorizationCorrectCases =
        {
            new object?[] { "userLoginNotFound", user1password, null },
            new object?[] { user1.Login, user1password, user1 },
            new object?[] { user2.Login, user2password, user2 },
        };
    }
}
