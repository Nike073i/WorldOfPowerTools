using NUnit.Framework;
using System.Threading.Tasks;
using WorldOfPowerTools.API.Test.Infrastructure.Helpers.Data;
using WorldOfPowerTools.DAL.Context;

namespace WorldOfPowerTools.API.Test.Controllers
{
    public abstract class ControllerBaseTests
    {
        protected WorldOfPowerToolsDb? _dbContext;

        [SetUp]
        public void SetUp()
        {
            var dbContextHelper = new DbContextHelper();
            _dbContext = dbContextHelper.DbContext;
        }

        [TearDown]
        public void TearDown()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            dbInitializer.RecreateDatabase();
        }

        protected async Task InitializeData()
        {
            var dbInitializer = new DbInitializer(_dbContext!);
            await dbInitializer!.InitializeAsync();
        }
    }
}
