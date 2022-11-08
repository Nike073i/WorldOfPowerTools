using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; protected set; }
        public double Price { get; protected set; }
        public Category Category { get; protected set; }
        public string Description { get; protected set; }
        public int Quantity { get; protected set; }
        public bool Availablity { get; protected set; }

#nullable disable
        protected Product() { }

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
