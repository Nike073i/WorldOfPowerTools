using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class Product : Entity
    {
        public static readonly int MinAvailableQuantity = 1;
        public static readonly int MinNameLength = 5;
        public static readonly int MaxNameLength = 100;
        public static readonly int MaxDescriptionLength = 2000;
        public static readonly double MinPrice = 1d;
        public static readonly double MaxPrice = 999_999_999d;
        public static readonly int MinQuantity = 0;
        public static readonly int MaxQuantity = 9999;

        private static readonly string RemoveMoreFromStoreThanExistsErrorMessage = "В наличии нет указанного количества товара";
        private static readonly string AddMoreToStoreThanMaxQuantityErrorMessage = "Количество товара при добавлении превышает ограничение максимального количества";

        private string _name;

        public string Name
        {
            get => _name;
            set
            {
                ThrowExceptionIfNameIncorrect(value);
                _name = value;
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                ThrowExceptionIfPriceIncorrect(value);
                _price = value;
            }
        }

        public Category Category { get; set; }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                ThrowExceptionIfDescriptionIncorrect(value);
                _description = value;
            }
        }

        public int Quantity { get; protected set; }
        public bool Availability { get; protected set; }

#nullable disable
        protected Product() { }

        public Product(string name, double price, Category category, string description, int quantity, bool availability = true)
        {
            ThrowExceptionIfNameIncorrect(name);
            ThrowExceptionIfPriceIncorrect(price);
            ThrowExceptionIfDescriptionIncorrect(description);
            ThrowExceptionIfQuantityIncorrect(quantity);

            Name = name;
            Price = price;
            Category = category;
            Description = description;
            Quantity = quantity;
            Availability = availability;
        }

        public Product AddToStore(int quantity)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            int newQuantity = Quantity + quantity;
            if (newQuantity > MaxQuantity) throw new InvalidOperationException(AddMoreToStoreThanMaxQuantityErrorMessage);
            Quantity = newQuantity;
            CheckAndSetAvailability(newQuantity);
            return this;
        }

        public Product RemoveFromStore(int quantity)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity));
            int newQuantity = Quantity - quantity;
            if (newQuantity < MinQuantity) throw new InvalidOperationException(RemoveMoreFromStoreThanExistsErrorMessage);
            Quantity = newQuantity;
            CheckAndSetAvailability(Quantity);
            return this;
        }

        private void CheckAndSetAvailability(int quantity)
        {
            Availability = quantity >= MinAvailableQuantity;
        }

        private void ThrowExceptionIfNameIncorrect(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (name.Length < MinNameLength || name.Length > MaxNameLength) throw new ArgumentOutOfRangeException(nameof(name));
        }

        private void ThrowExceptionIfPriceIncorrect(double price)
        {
            if (price < MinPrice || price > MaxPrice) throw new ArgumentOutOfRangeException(nameof(price));
        }

        private void ThrowExceptionIfDescriptionIncorrect(string description)
        {
            if (string.IsNullOrEmpty(description)) throw new ArgumentNullException(nameof(description));
            if (description.Length > MaxDescriptionLength) throw new ArgumentOutOfRangeException(nameof(description));
        }

        private void ThrowExceptionIfQuantityIncorrect(int quantity)
        {
            if (quantity < MinQuantity || quantity > MaxQuantity) throw new ArgumentOutOfRangeException(nameof(quantity));
        }
    }
}
