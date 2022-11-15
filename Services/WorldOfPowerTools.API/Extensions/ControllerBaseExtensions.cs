using Microsoft.AspNetCore.Mvc;
using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Services;

namespace WorldOfPowerTools.API.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static bool IsAvailableRequest(this ControllerBase controllerBase, SecurityService securityService, Actions access)
        {
            var userRights = controllerBase.User.GetUserRights();
            if (userRights == null) return false;
            if (!securityService.UserOperationAvailability(userRights.Value, access)) return false;
            return true;
        }
    }
}
