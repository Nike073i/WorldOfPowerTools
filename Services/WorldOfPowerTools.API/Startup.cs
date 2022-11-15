using Microsoft.EntityFrameworkCore;
using WorldOfPowerTools.DAL.Context;
using WorldOfPowerTools.DAL.Repositories;
using WorldOfPowerTools.Domain.Repositories;

namespace WorldOfPowerTools.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnection = Configuration.GetSection("Database").GetConnectionString("MSSQL");
            services.AddDbContext<WorldOfPowerToolsDb>(options => options.UseSqlServer(dbConnection, x => x.MigrationsAssembly("WorldOfPowerTools.DAL.SqlServer")));
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddTransient<IProductRepository, DbProductRepository>();
            services.AddTransient<IOrderRepository, DbOrderRepository>();
            services.AddTransient<ICartLineRepository, DbCartLineRepository>();
            services.AddTransient<IUserRepository, DbUserRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, WorldOfPowerToolsDb context)
        {
            if (env.IsDevelopment())
            {
                context.Database.Migrate();
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}