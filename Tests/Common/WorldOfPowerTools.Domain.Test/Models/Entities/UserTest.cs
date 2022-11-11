using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Test.Models.Entities
{
    public class UserTest
    {
        [Test]
        [TestCaseSource(nameof(IncorrectConstructCases))]
        public void CreateUserIncorrect(string login, string passwordHash, Actions rights, Type awaitingException)
        {
            TestDelegate construct = () => new User(login, passwordHash, rights);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        public void CreateUserCorrect()
        {
            var expProduct = CreateUser();
            var personData = CreateUser();
            Assert.True(IsEqualUsers(expProduct, personData));
        }

        [Test]
        [TestCaseSource(nameof(AllowActionCases))]
        public void AllowAction(Actions userActions, Actions allowedActions, Actions newUserActions)
        {
            var expUser = CreateUser(actions: newUserActions);
            var user = CreateUser(actions: userActions);
            user.AllowAction(allowedActions);
            Assert.True(IsEqualUsers(expUser, user));
        }

        [Test]
        [TestCaseSource(nameof(ProhibitActionCases))]
        public void ProhibitAction(Actions userActions, Actions prohibitedActions, Actions newUserActions)
        {
            var expUser = CreateUser(actions: newUserActions);
            var user = CreateUser(actions: userActions);
            user.ProhibitAction(prohibitedActions);
            Assert.True(IsEqualUsers(expUser, user));
        }

        private User CreateUser(string login = "Тестовый логин", string passwordHash = "9363fc3c6345251ffe019a3365447febab2b649de3643205eeec1053126ad749", Actions actions = Actions.None)
        {
            return new User(login, passwordHash, actions);
        }

        private bool IsEqualUsers(User a, User b)
        {
            return a.Login == b.Login &&
                a.PasswordHash == b.PasswordHash &&
                a.PersonData == b.PersonData &&
                a.ContactData == b.ContactData &&
                a.Address == b.Address &&
                a.Rights == b.Rights;
        }

        static readonly object[] IncorrectConstructCases =
        {
            new object?[] { "", "9363fc3c6345251ffe019a3365447febab2b649de3643205eeec1053126ad749", Actions.None, typeof(ArgumentNullException) },
            new object?[] { null, "9363fc3c6345251ffe019a3365447febab2b649de3643205eeec1053126ad749", Actions.None, typeof(ArgumentNullException) },
            new object?[] { "".PadLeft(User.MinLoginLength - 1), "9363fc3c6345251ffe019a3365447febab2b649de3643205eeec1053126ad749", Actions.None, typeof(ArgumentOutOfRangeException) },
            new object?[] { "".PadLeft(User.MaxLoginLength + 1), "9363fc3c6345251ffe019a3365447febab2b649de3643205eeec1053126ad749", Actions.None, typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовый логин", "", Actions.None, typeof(ArgumentNullException) },
            new object?[] { "Тестовый логин", null, Actions.None, typeof(ArgumentNullException) },
        };

        static readonly object[] AllowActionCases =
        {
            new object[] { Actions.None, Actions.MyOrders, Actions.MyOrders },
            new object[] { Actions.MyOrders | Actions.Cart, Actions.MyOrders, Actions.MyOrders | Actions.Cart },
            new object[] { Actions.MyOrders | Actions.Cart, Actions.Products, Actions.MyOrders | Actions.Cart | Actions.Products }
        };

        static readonly object[] ProhibitActionCases =
{
            new object[] { Actions.None, Actions.MyOrders, Actions.None },
            new object[] { Actions.MyOrders | Actions.Cart, Actions.MyOrders, Actions.Cart },
            new object[] { Actions.MyOrders | Actions.Cart | Actions.Products, Actions.MyOrders | Actions.Cart | Actions.Products, Actions.None }
        };
    }
}
