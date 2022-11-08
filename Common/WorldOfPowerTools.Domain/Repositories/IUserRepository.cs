using WorldOfPowerTools.Domain.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public User? GetByLoginAsync(string login);
    }
}
