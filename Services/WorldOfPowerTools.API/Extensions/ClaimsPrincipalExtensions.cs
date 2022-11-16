using System.Security.Claims;
using WorldOfPowerTools.Domain.Enums;

namespace WorldOfPowerTools.API.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public const string CLAIM_USER_GUID = "guid";
        public const string CLAIM_USER_RIGHTS = "rights";

        public static Guid? GetUserId(this ClaimsPrincipal user)
        {
            if (Guid.TryParse(user.FindFirstValue(CLAIM_USER_GUID), out var userUuid))
            {
                return userUuid;
            }

            return null;
        }

        public static Actions? GetUserRights(this ClaimsPrincipal user)
        {
            if (Enum.TryParse(typeof(Actions), user.FindFirstValue(CLAIM_USER_RIGHTS), out var userRights))
            {
                return (Actions?)userRights;
            }
            return null;
        }
    }
}
