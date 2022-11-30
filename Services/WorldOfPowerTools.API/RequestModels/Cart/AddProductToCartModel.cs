using System.ComponentModel.DataAnnotations;

namespace WorldOfPowerTools.API.RequestModels.Cart
{
    public class AddProductToCartModel
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
        [Required]
        public int Quantity { get; set; }
    }
}
