using System;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities
{
    public static class CartLineHelper
    {
        public static readonly Guid TestProductId = new("9a072eac-f6d3-40d3-a7d5-46ec1e8a34f4");
        public static readonly Guid TestUserId = new("14c0dd99-93b0-4450-bafd-f92f6cd2dff7");
        public const int TestCartLineProductQuantity = 50;

        public static CartLine CreateCartLines(Guid? userId = null, Guid? productId = null, int quantity = TestCartLineProductQuantity)
        {
            userId ??= TestUserId;
            productId ??= TestProductId;
            return new CartLine(userId.Value, productId.Value, quantity);
        }
    }
}
