using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemService.Extensions;
using SystemService.QuartzSchedule;
using SystemService.Workers;

namespace SystemService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSystemd()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddNlog(hostContext.Configuration);
                    services.AddQuartz(hostContext.Configuration);
                    services.AddHostedService<QuartzWorker>();
                });
    }
}
