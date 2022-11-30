using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.API.RequestModels.Product
{
    public class UpdateProductModel
    {
        [Required]
        public Guid ProductId { get; set; }

        public string? Name { get; set; } = null;

        public double? Price { get; set; } = null;

        public string? Description { get; set; } = null;

        public Category? Category { get; set; } = null;
    }
}
