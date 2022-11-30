using System.ComponentModel.DataAnnotations;

namespace WorldOfPowerTools.API.RequestModels.Cart
{
    public class RemoveProductModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        public int? Quantity { get; set; } = null;
    }
}
