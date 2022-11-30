using System.ComponentModel.DataAnnotations;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.API.RequestModels.User
{
    public class ChangeUserRightModel
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Actions Action { get; set; }
    }
}
