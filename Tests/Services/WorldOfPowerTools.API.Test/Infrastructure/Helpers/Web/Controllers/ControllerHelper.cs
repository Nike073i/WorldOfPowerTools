using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Controllers
{
    public static class ControllerHelper
    {
        public static void SetControllerContext(in ControllerBase controller, ClaimsPrincipal user)
        {
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext { User = user };
        }
    }
}
