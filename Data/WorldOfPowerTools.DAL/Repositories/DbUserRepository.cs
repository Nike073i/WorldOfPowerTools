using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbUserRepository : IUserRepository
    {
        public IEnumerable<User> GetAllAsync(int? skip, int? take)
        {
            throw new NotImplementedException();
        }

        public User GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public User GetByLoginAsync(string login)
        {
            throw new NotImplementedException();
        }

        public Guid RemoveByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public User SaveAsync(User entity)
        {
            throw new NotImplementedException();
        }
    }
}