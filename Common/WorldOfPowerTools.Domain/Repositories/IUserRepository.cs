using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.Domain.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        public Task<User?> GetByLoginAsync(string login);
    }
}
