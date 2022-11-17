using System;
using System.Collections.Generic;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities
{
    public static class OrderHelper
    {
        public static readonly Guid TestUserId = new("d0fc73e0-f04d-4254-b54a-d92d1e742bad");
        public static readonly Address TestAddress = new("country1", "city1", "house1", "flat1", 15, "111111");
        public static readonly ContactData TestContactData = new("+79176316458", "kit1073i@miif.ry");
        public const double TestOrderPrice = 100;

        public static Order CreateOrder(IEnumerable<CartLine> cartLine, Guid? userId = null, double price = TestOrderPrice, Address? address = null, ContactData? contactData = null)
        {
            userId ??= TestUserId;
            address ??= TestAddress;
            contactData ??= TestContactData;
            return new Order(userId.Value, price, address, contactData, cartLine);
        }
    }
}