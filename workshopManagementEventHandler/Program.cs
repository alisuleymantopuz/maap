﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using infrastructure.messaging;
using workshopManagementEventHandler.DataAccess;
using Polly;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;


namespace workshopManagementEventHandler
{
    class Program
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
                    services.AddTransient<IMessageHandler>((svc) =>
                    {
                        var rabbitMQConfigSection = hostContext.Configuration.GetSection("RabbitMQ");
                        string rabbitMQHost = rabbitMQConfigSection["Host"];
                        string rabbitMQUserName = rabbitMQConfigSection["UserName"];
                        string rabbitMQPassword = rabbitMQConfigSection["Password"];
                        return new RabbitMQMessageHandler(rabbitMQHost, rabbitMQUserName, rabbitMQPassword, "Maap", "WorkshopManagement", ""); ;
                    });

                    services.AddTransient<WorkshopManagementDBContext>((svc) =>
                    {
                        var sqlConnectionString = hostContext.Configuration.GetConnectionString("WorkshopManagementCN");
                        var dbContextOptions = new DbContextOptionsBuilder<WorkshopManagementDBContext>()
                            .UseSqlServer(sqlConnectionString)
                            .Options;
                        var dbContext = new WorkshopManagementDBContext(dbContextOptions);

                        Policy
                            .Handle<Exception>()
                            .WaitAndRetry(10, r => TimeSpan.FromSeconds(10), (ex, ts) => { Log.Error("Error connecting to DB. Retrying in 10 sec."); })
                            .Execute(() => DBInitializer.Initialize(dbContext));

                        return dbContext;
                    });

                    services.AddHostedService<EventHandler>();
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();

            return hostBuilder;
        }
    }
}
