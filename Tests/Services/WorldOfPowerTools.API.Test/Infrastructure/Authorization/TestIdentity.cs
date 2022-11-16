using System.Security.Claims;

namespace WorldOfPowerTools.API.Test.Infrastructure.Authorization
{
    public class TestIdentity : ClaimsIdentity
    {
        public TestIdentity(params Claim[] claims) : base(claims) { }
    }
}
