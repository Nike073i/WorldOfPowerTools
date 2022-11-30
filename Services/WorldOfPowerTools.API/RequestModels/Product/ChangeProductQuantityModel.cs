using System.ComponentModel.DataAnnotations;

namespace WorldOfPowerTools.API.RequestModels.Product
{
    public class ChangeProductQuantityModel
    {
        [Required]
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
