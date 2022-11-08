using WorldOfPowerTools.Domain.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public class IUserRepository
    {
        public User? GetByLoginAsync(string login)
        {
            throw new System.Exception("Not implemented");
        }
    }
}
