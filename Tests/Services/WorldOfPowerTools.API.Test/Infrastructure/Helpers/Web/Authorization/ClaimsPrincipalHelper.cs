using System;
using System.Collections.Generic;
using System.Security.Claims;
using WorldOfPowerTools.API.Extensions;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Authorization
{
    public static class ClaimsPrincipalHelper
    {
        public static ClaimsPrincipal CreateUser(Guid? userId = null, Actions? userRights = null)
        {
            var claims = new List<Claim>();
            if (userId.HasValue) claims.Add(new Claim(ClaimsPrincipalExtensions.CLAIM_USER_GUID, userId.Value.ToString()));
            if (userRights.HasValue) claims.Add(new Claim(ClaimsPrincipalExtensions.CLAIM_USER_RIGHTS, userRights.Value.ToString()));

            return new TestPrincipal(claims.ToArray());
        }
    }
}
