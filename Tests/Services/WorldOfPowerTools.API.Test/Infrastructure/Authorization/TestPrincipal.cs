using System.Security.Claims;

namespace WorldOfPowerTools.API.Test.Infrastructure.Authorization
{
    public class TestPrincipal : ClaimsPrincipal
    {
        public TestPrincipal(params Claim[] claims) : base(new TestIdentity(claims)) { }
    }
}
