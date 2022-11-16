using System.Security.Claims;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Web.Authorization
{
    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims) { }
    }
}
