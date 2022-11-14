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

        public async Task<bool> UserOperationAvailability(Guid userId, Actions action)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException(nameof(userId));
            var user = await _userRepository.GetByIdAsync(userId);
            return user != null && user.Rights.IsSet(action);
        }
    }
}