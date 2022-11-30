using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.API.RequestModels.Product
{
    public class AddProductModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int Quantity { get; set; }

        public Category Category { get; set; } = Category.Screwdriver;
        public bool Availability { get; set; } = true;
    }
}
