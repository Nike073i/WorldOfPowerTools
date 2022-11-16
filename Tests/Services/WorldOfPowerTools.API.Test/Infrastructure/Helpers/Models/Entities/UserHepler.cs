using System;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities
{
    public static class UserHepler
    {
        public static readonly Guid TestUserId = new("76831ddb-61be-4778-ad77-2712b7c828eb");
        public const string TestUserLogin = "testUserLogin";
        public const string TestUserPasswordHash = "testUserPasswordHash";
        public const Actions TestUserRights = Actions.Cart | Actions.MyOrders;

        public static User CreateUser(string login = TestUserLogin, string passwordHasd = TestUserPasswordHash, Actions userRights = TestUserRights)
        {
            return new User(login, passwordHasd, userRights);
        }
    }
}
