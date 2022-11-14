using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.Domain.Models.Entities;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.DAL.Repositories
{
    public class DbUserRepository : DbRepository<User>, IUserRepository
    {
        public DbUserRepository(WorldOfPowerToolsDb context) : base(context) { }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await Items.FirstOrDefaultAsync(user => user.Login == login);
        }
    }
}