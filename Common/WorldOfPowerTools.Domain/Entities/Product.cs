using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Category Category { get; set; }
        public string Description { get; set; }
        public int Quantity { get; private set; }
        public bool Availablity { get; private set; }

        public Product(string name, double price, Category category, string description, int quantity, bool availability = true)
        {
            throw new System.Exception("Not implemented");
        }
        public Product AddToStore(int quantity)
        {
            throw new System.Exception("Not implemented");
        }
        public Product RemoveFromStore(int quantity)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
