using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context.Configurations;

namespace WorldOfPowerTools.DAL.Context
{
    public class WorldOfPowerToolsDb : DbContext
    {
        public WorldOfPowerToolsDb(DbContextOptions<WorldOfPowerToolsDb> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new CartLineConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
        }
    }
}