using System.IO;
using System.Threading.Tasks;
using infrastructure.messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace auditLogService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddEnvironmentVariables("DOTNET_");
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: false);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    RabbitMQ(hostContext, services);
                    AuditLogManager(hostContext, services);
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();

            return hostBuilder;
        }

        private static void AuditLogManager(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddTransient<AuditlogManagerConfig>((svc) =>
            {
                var auditlogConfigSection = hostContext.Configuration.GetSection("Auditlog");
                string logPath = auditlogConfigSection["path"];
                return new AuditlogManagerConfig { LogPath = logPath };
            });

            services.AddHostedService<AuditLogManager>();
        }

        private static void RabbitMQ(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddTransient<IMessageHandler>((svc) =>
            {
                var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                string rabbitMQHost = rabbitMQConfigSection["Host"];
                string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                string rabbitMQPassword = rabbitMQConfigSection["Password"];
                return new RabbitMQMessageHandler(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "Maap", "Auditlog", "");
            });
        }
    }
}
