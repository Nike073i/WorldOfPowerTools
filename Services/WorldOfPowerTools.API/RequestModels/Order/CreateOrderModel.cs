using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.Domain.Models.ObjectValues;

namespace WorldOfPowerTools.API.RequestModels.Order
{
    public class CreateOrderModel
    {
        [Required]
        public Guid UserId { get; set; }

        [FromQuery]
        [Required]
        public Address Address { get; set; }

        [FromQuery]
        [Required]
        public ContactData ContactData { get; set; }
    }
}
