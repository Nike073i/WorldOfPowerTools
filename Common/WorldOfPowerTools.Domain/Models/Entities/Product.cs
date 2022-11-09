using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Models.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public int Quantity { get; protected set; }
        public bool Availablity { get; protected set; }

#nullable disable
        protected Product() { }

        public Product(string name, double price, Category category, string description, int quantity, bool availability = true)
        {
            throw new Exception("Not implemented");
        }
        public Product AddToStore(int quantity)
        {
            throw new Exception("Not implemented");
        }
        public Product RemoveFromStore(int quantity)
        {
            throw new Exception("Not implemented");
        }
    }
}
