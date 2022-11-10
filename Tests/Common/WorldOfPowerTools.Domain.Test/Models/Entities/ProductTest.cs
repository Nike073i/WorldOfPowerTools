using NUnit.Framework;
using System;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Test.Models.Entities
{
    public class ProductTest
    {
        [Test]
        [TestCaseSource(nameof(IncorrectConstructCases))]
        public void CreateProductIncorrect(string name, double price, Category category, string description, int quantity, Type awaitingException)
        {
            TestDelegate construct = () => new Product(name, price, category, description, quantity);
            Assert.Throws(awaitingException, construct);
        }

        [Test]
        public void CreateProductCorrect()
        {
            var expProduct = CreateProduct();
            var personData = CreateProduct();
            Assert.True(IsEqualProducts(expProduct, personData));
        }

        [Test]
        [TestCaseSource(nameof(IncorrectSetNameCases))]
        public void SetNameIncorrectValue(string value, Type awaitingException)
        {
            var product = CreateProduct();
            Assert.Throws(awaitingException, () => product.Name = value);
        }

        [Test]
        [TestCaseSource(nameof(CorrectSetNameCases))]
        public void SetNameСorrectValue(string value)
        {
            string newName = value;
            var expProduct = CreateProduct(name: newName);
            var product = CreateProduct();
            product.Name = newName;
            Assert.True(IsEqualProducts(expProduct, product));
        }

        [Test]
        [TestCaseSource(nameof(IncorrectSetPriceCases))]
        public void SetPriceIncorrectValue(double value, Type awaitingException)
        {
            var product = CreateProduct();
            Assert.Throws(awaitingException, () => product.Price = value);
        }

        [Test]
        [TestCaseSource(nameof(CorrectSetPriceCases))]
        public void SetPriceСorrectValue(double value)
        {
            double newPrice = value;
            var expProduct = CreateProduct(price: newPrice);
            var product = CreateProduct();
            product.Price = newPrice;
            Assert.True(IsEqualProducts(expProduct, product));
        }

        [Test]
        public void SetCategoryCorrectValue()
        {
            var newCategory = Category.Screwdriver;
            var expProduct = CreateProduct(category: newCategory);
            var product = CreateProduct();
            product.Category = newCategory;
            Assert.True(IsEqualProducts(expProduct, product));
        }

        [Test]
        [TestCaseSource(nameof(IncorrectSetDescriptionCases))]
        public void SetDescriptionIncorrectValue(string value, Type awaitingException)
        {
            var product = CreateProduct();
            Assert.Throws(awaitingException, () => product.Description = value);
        }

        [Test]
        [TestCaseSource(nameof(CorrectSetDescriptionCases))]
        public void SetDescriptionСorrectValue(string value)
        {
            string newDescription = value;
            var expProduct = CreateProduct(description: newDescription);
            var product = CreateProduct();
            product.Description = newDescription;
            Assert.True(IsEqualProducts(expProduct, product));
        }

        [Test]
        [TestCaseSource(nameof(IncorrectAdditionCases))]
        public void AddToStoreIncorrectValue(int value, Type awaitingException)
        {
            var product = CreateProduct();
            Assert.Throws(awaitingException, () => product.AddToStore(value));
        }

        [Test]
        [TestCaseSource(nameof(AddMoreFromStoreThanMaxQuantityCases))]
        public void AddMoreFromStoreThanMaxQuantity(int oldQuantity, int addition, Type awaitingException)
        {
            var product = CreateProduct(quantity: oldQuantity);
            Assert.Throws(awaitingException, () => product.AddToStore(addition));
        }

        [Test]
        [TestCaseSource(nameof(CorrectAdditionCases))]
        public void AddToStoreCorrectValue(int oldQuantity, int addition, int newQuantity)
        {
            var expProduct = CreateProduct(quantity: newQuantity);
            var product = CreateProduct(quantity: oldQuantity);
            product.AddToStore(addition);
            Assert.True(IsEqualProducts(expProduct, product));
        }

        [Test]
        [TestCaseSource(nameof(IncorrectRemovalCases))]
        public void RemoveFromStoreIncorrectValue(int value, Type awaitingException)
        {
            var product = CreateProduct();
            Assert.Throws(awaitingException, () => product.RemoveFromStore(value));
        }

        [Test]
        [TestCase(100, 101, typeof(InvalidOperationException))]
        public void RemoveMoreFromStoreThanExists(int oldQuantity, int removal, Type awaitingException)
        {
            var product = CreateProduct(quantity: oldQuantity);
            Assert.Throws(awaitingException, () => product.RemoveFromStore(removal));
        }

        [Test]
        [TestCaseSource(nameof(CorrectRemovalCases))]
        public void RemoveFromStoreCorrectValue(int oldQuantity, int removal, int newQuantity)
        {
            var expProduct = CreateProduct(quantity: newQuantity);
            var product = CreateProduct(quantity: oldQuantity);
            product.RemoveFromStore(removal);
            Assert.True(IsEqualProducts(expProduct, product));
        }

        [Test]
        public void AvailabilityChangeAfterProductEnded()
        {
            var expProduct = CreateProduct(quantity: Product.MinQuantity, isAvailable : false);
            var product = CreateProduct(quantity: 100);
            product.RemoveFromStore(100);
            Assert.True(IsEqualProducts(expProduct, product));
        }

        [Test]
        public void AvailabilityChangeAfterProductAppeared()
        {
            var expProduct = CreateProduct(quantity: Product.MinQuantity + 150, isAvailable: true);
            var product = CreateProduct(quantity: Product.MinQuantity);
            product.AddToStore(150);
            Assert.True(IsEqualProducts(expProduct, product));
        }

        private Product CreateProduct(string name = "Тестовый товар", string description = "Тестовое описание",
            double price = 150d, Category category = Category.Painting, int quantity = 100, bool isAvailable = true)
        {
            return new Product(name, price, category, description, quantity, isAvailable);
        }

        private bool IsEqualProducts(Product a, Product b)
        {
            return a.Name == b.Name &&
                a.Price == b.Price &&
                a.Category == b.Category &&
                a.Description == b.Description &&
                a.Price == b.Price &&
                a.Quantity == b.Quantity &&
                a.Availability == b.Availability;
        }

        static readonly object[] IncorrectConstructCases =
        {
            new object?[] { "", 150d, Category.Painting, "Тестовое описание", 100, typeof(ArgumentNullException) },
            new object?[] { null, 150d, Category.Painting, "Тестовое описание", 100, typeof(ArgumentNullException) },
            new object?[] { "".PadLeft(Product.MinNameLength - 1), 150d, Category.Painting, "Тестовое описание", 100, typeof(ArgumentOutOfRangeException) },
            new object?[] { "".PadLeft(Product.MaxNameLength + 1), 150d, Category.Painting, "Тестовое описание", 100, typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовый товар", Product.MinPrice - 1, Category.Painting, "Тестовое описание", 100, typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовый товар", Product.MaxPrice + 1, Category.Painting, "Тестовое описание", 100, typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовый товар", 150d, Category.Painting, "", 100, typeof(ArgumentNullException) },
            new object?[] { "Тестовый товар", 150d, Category.Painting, null, 100, typeof(ArgumentNullException) },
            new object?[] { "Тестовый товар", 150d, Category.Painting, "".PadLeft(Product.MaxDescriptionLength + 1, '-'), 100, typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовый товар", 150d, Category.Painting, "Тестовое описание", Product.MinQuantity - 1, typeof(ArgumentOutOfRangeException) },
            new object?[] { "Тестовый товар", 150d, Category.Painting, "Тестовое описание", Product.MaxQuantity + 1, typeof(ArgumentOutOfRangeException) },
        };

        static readonly object[] IncorrectSetNameCases =
        {
            new object?[] { "", typeof(ArgumentNullException) },
            new object?[] { null, typeof(ArgumentNullException) },
            new object?[] { "".PadLeft(Product.MinNameLength - 1), typeof(ArgumentOutOfRangeException) },
            new object?[] { "".PadLeft(Product.MaxNameLength + 1), typeof(ArgumentOutOfRangeException) }
        };

        static readonly object[] CorrectSetNameCases =
{
            "".PadLeft(Product.MinNameLength),
            "".PadLeft(Product.MinNameLength + 1),
            "".PadLeft(Product.MaxNameLength),
            "".PadLeft(Product.MaxNameLength - 1)
        };

        static readonly object[] IncorrectSetPriceCases =
{
            new object?[] { -Product.MaxPrice, typeof(ArgumentOutOfRangeException) },
            new object?[] { Product.MinPrice - 1, typeof(ArgumentOutOfRangeException) },
            new object?[] { Product.MaxPrice + 1, typeof(ArgumentOutOfRangeException) },
        };

        static readonly object[] CorrectSetPriceCases =
        {
            Product.MinPrice,
            Product.MaxPrice,
        };

        static readonly object[] IncorrectSetDescriptionCases =
{
            new object?[] { "", typeof(ArgumentNullException) },
            new object?[] { null, typeof(ArgumentNullException) },
            new object?[] { "".PadLeft(Product.MaxDescriptionLength + 1), typeof(ArgumentOutOfRangeException) },
        };

        static readonly object[] CorrectSetDescriptionCases =
        {
            "d",
            "".PadLeft(Product.MaxDescriptionLength - 1),
        };

        static readonly object[] IncorrectAdditionCases =
        {
            new object?[] { -Product.MaxQuantity, typeof(ArgumentOutOfRangeException) },
            new object?[] { -1, typeof(ArgumentOutOfRangeException) },
            new object?[] { Product.MaxQuantity + 1, typeof(InvalidOperationException) },
        };

        static readonly object[] CorrectAdditionCases =
        {
            new object[] { Product.MinQuantity, 100, 100 + Product.MinQuantity },
            new object[] { Product.MinQuantity, Product.MaxQuantity - Product.MinQuantity, Product.MaxQuantity, },
        };

        static readonly object[] AddMoreFromStoreThanMaxQuantityCases =
        {
            new object[] { 100, Product.MaxQuantity, typeof(InvalidOperationException) }
        };

        static readonly object[] IncorrectRemovalCases =
{
            new object?[] { -Product.MaxQuantity, typeof(ArgumentOutOfRangeException) },
            new object?[] { -1, typeof(ArgumentOutOfRangeException) },
            new object?[] { Product.MaxQuantity + 1, typeof(InvalidOperationException) },
        };

        static readonly object[] CorrectRemovalCases =
        {
            new object[] { Product.MinQuantity + 101, 100, Product.MinQuantity + 1 },
            new object[] { Product.MaxQuantity, Product.MaxQuantity - Product.MinQuantity - 1, Product.MinQuantity + 1, },
        };
    }
}
