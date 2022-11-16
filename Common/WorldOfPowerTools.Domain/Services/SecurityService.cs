using WorldOfPowerTools.Domain.Enums;
using WorldOfPowerTools.Domain.Extensions;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class SecurityService
    {
        private readonly IUserRepository _userRepository;

        public SecurityService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> UserOperationAvailability(Guid? userId, Actions action)
        {
            if (!userId.HasValue || userId.Value == Guid.Empty) return false;
            var user = await _userRepository.GetByIdAsync(userId.Value);
            return user != null && UserOperationAvailability(user.Rights, action);
        }

        public bool UserOperationAvailability(Actions? userRights, Actions action)
        {
            return userRights.HasValue && userRights.Value.IsSet(action);
        }

        public bool IsIndividualOperation(Guid? userId, Guid awaitingUserId)
        {
            return userId != null && userId == awaitingUserId;
        }
    }
}