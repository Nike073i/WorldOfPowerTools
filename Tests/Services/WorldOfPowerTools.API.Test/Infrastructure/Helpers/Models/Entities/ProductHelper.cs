using System;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Models.Entities
{
    public static class ProductHelper
    {
        public static readonly Guid TestProductId = new("9e0e3e81-b089-4ccb-9775-bded1cd39198");
        public const string TestProductName = "testProductName";
        public const double TestProductPrice = 100;
        public const Category TestProductCategory = Category.Perforator;
        public const string TestProductDescription = "testProductDescription";
        public const int TestProductQuantity = 50;
        public const bool TestProductAvailability = true;

        public static Product CreateProduct(string name = TestProductName, double price = TestProductPrice, Category category = TestProductCategory, string description = TestProductDescription,
            int quantity = TestProductQuantity, bool availability = TestProductAvailability)
        {
            return new Product(name, price, category, description, quantity, availability);
        }
    }
}
