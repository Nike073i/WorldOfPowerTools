using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorldOfPowerTools.DAL.Context;

namespace WorldOfPowerTools.API.Test.Infrastructure.Helpers.Data
{
    public class DbContextHelper
    {
        public WorldOfPowerToolsDb DbContext { get; private set; }
        public DbContextHelper()
        {
            var builder = new DbContextOptionsBuilder<WorldOfPowerToolsDb>();
            //builder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WorldOfPowerToolsDbTest;Integrated Security=True;Multiple Active Result Sets=True;", x => x.MigrationsAssembly("WorldOfPowerTools.DAL.SqlServer"));
            builder.UseInMemoryDatabase("INTEGRATION_TESTING").ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            var options = builder.Options;
            DbContext = new WorldOfPowerToolsDb(options);
        }
    }
}
