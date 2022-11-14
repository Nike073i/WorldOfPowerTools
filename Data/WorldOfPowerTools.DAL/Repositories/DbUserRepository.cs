using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbUserRepository : DbRepository<User>, IUserRepository
    {
        public DbUserRepository(WorldOfPowerToolsDb context) : base(context) { }

        public Task<User?> GetByLoginAsync(string login)
        {
            throw new NotImplementedException();
        }
    }
}