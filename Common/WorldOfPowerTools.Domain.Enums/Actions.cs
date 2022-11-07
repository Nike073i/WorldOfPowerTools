using System;
namespace WorldOfPowerTools.Domain.Enums
{
    [Flags]
    public enum Actions
    {
        None = 0,
        Cart = 0x0001,
        MyOrders = 0x0002,
        Products = 0x0004,
        AllOrders = 0x0008,
        Users = 0x0010,
    }
}
