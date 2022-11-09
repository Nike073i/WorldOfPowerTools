using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context.Configurations;
using WorldOfPowerTools.Domain.Models.Entities;

namespace WorldOfPowerTools.DAL.Context
{
    public class WorldOfPowerToolsDb : DbContext
    {
        public WorldOfPowerToolsDb(DbContextOptions<WorldOfPowerToolsDb> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
        }
    }
}