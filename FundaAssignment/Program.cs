using System;
using System.IO;
using System.Threading.Tasks;
using FundaAssignment.Interfaces;
using FundaAssignment.Model;
using FundaAssignment.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FundaAssignment
{
    public class Program
    {
        public static IConfiguration Configuration;

        private static async Task Main(string[] args)
        {
            IHost host = ConfigureServices(args).Build();
            IOrchestrationService operations = host.Services.GetRequiredService<IOrchestrationService>();

            try
            {
                await operations.Execute(withTuin: false);
                await operations.Execute(withTuin: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Something went wrong: {ex}");
            }
        }

        private static IHostBuilder ConfigureServices(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, serviceCollection) =>
            {
                serviceCollection.AddTransient<IOrchestrationService, OrchestrationService>();
                serviceCollection.AddTransient<IHttpClientService, HttpClientService>();
                serviceCollection.AddTransient<IConsoleService, ConsoleService>();

                Configuration = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                                        .AddJsonFile("appsettings.json", false)
                                        .Build();

                serviceCollection.Configure<Configuration>(Configuration.GetSection("Api"));
                serviceCollection.AddLogging(configure => configure.AddConsole()).AddTransient<Program>();
            });
    }
}