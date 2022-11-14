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
            services.AddDbContext<WorldOfPowerToolsDb>(options => options.UseSqlServer(dbConnection));
            services.AddControllers();
            services.AddSwaggerGen();
            services.AddTransient<IProductRepository, DbProductRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
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