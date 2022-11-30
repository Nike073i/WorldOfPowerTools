using System.ComponentModel.DataAnnotations;

namespace WorldOfPowerTools.API.RequestModels.Identity
{

    public class IdentityModel
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
