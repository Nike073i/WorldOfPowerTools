using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.TestConsole
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string? dbConnection = Configuration.GetSection("Database").GetConnectionString("MSSQL");
            if (dbConnection == null) throw new Exception("Не найдена строка подключения: appsettings.json Database:MSSQL");
            services.AddDbContext<WorldOfPowerToolsDb>(options => options.UseSqlServer(dbConnection, x => x.MigrationsAssembly("WorldOfPowerTools.DAL.SqlServer")));
            services.AddTransient<IProductRepository, DbProductRepository>();
            services.AddTransient<IOrderRepository, DbOrderRepository>();
            services.AddTransient<IUserRepository, DbUserRepository>();
        }
    }
}