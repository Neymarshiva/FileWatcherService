using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FileWatcherService.Models;
using FileWatcherService.Services;
using FileWatcherService.Utilities;
using FileWatcherService;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                services.Configure<FileWatcherSettings>(context.Configuration.GetSection("FileWatcherSettings"));
                services.AddSingleton<IFileProcessor, FileProcessor>();
                services.AddSingleton<IXmlParser, XmlParser>();
                services.AddHostedService<Worker>();
            })
            .UseSerilog((context, services, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext() 
                    .WriteTo.Console()
                    .WriteTo.File(
                        path: "logs/log-.txt",
                        rollingInterval: RollingInterval.Day,
                        rollOnFileSizeLimit: true,
                        formatter: new Serilog.Formatting.Compact.CompactJsonFormatter()
                    );
            });
}
