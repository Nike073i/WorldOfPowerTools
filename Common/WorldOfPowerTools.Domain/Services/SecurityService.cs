using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class SecurityService
    {
        private readonly IUserRepository _userRepository;
        public SecurityService(IUserRepository userRepository)
        {
            throw new System.Exception("Not implemented");
        }
        public bool UserOperationAvailability(Guid userId, Actions access)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
