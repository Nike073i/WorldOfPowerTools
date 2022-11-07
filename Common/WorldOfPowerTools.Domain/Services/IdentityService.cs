using WorldOfPowerTools.Domain.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.Domain.Services
{
    public class IdentityService
    {
        private readonly IUserRepository _userRepository;

        public IdentityService(IUserRepository userRepository)
        {
            throw new System.Exception("Not implemented");
        }

        public User? Authorization(string login, string password)
        {
            throw new System.Exception("Not implemented");
        }
        public User? Registration(string login, string password)
        {
            throw new System.Exception("Not implemented");
        }
        public string PasswordHash(string password)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
