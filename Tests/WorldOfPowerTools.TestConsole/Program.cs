using Microsoft.Extensions.Hosting;

namespace WorldOfPowerTools.TestConsole
{
    public class Program
    {
        private static IHost _host { get; set; }
        public static async Task Main(string[] args)
        {
            _host = CreateHostBuilder(args).Build();
            await _host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args).ConfigureServices((_, services) =>
                {
                    var startup = new Startup();
                    startup.ConfigureServices(services);
                });
        }
    }
}