using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbUserRepository : IUserRepository
    {
        public Task<IEnumerable<User>> GetAllAsync(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByLoginAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> RemoveByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<User> SaveAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}