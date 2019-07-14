using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using infrastructure.messaging;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using workshopManagementAPI.Repositories;
using workshopManagementAPI.Commands;
using workshopManagementAPI.Events;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using workshopManagementAPI.CommandHandlers;

namespace workshopManagementAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // add repo classes
            var eventStoreConnectionString = Configuration.GetConnectionString("EventStoreCN");
            services.AddTransient<IWorkshopPlanningRepository>((sp) =>
                new SqlServerWorkshopPlanningRepository(eventStoreConnectionString));

            var workshopManagementConnectionString = Configuration.GetConnectionString("WorkshopManagementCN");
            services.AddTransient<IVehicleRepository>((sp) => new SqlServerRefDataRepository(workshopManagementConnectionString));
            services.AddTransient<ICustomerRepository>((sp) => new SqlServerRefDataRepository(workshopManagementConnectionString));

            // add messagepublisher classes
            var configSection = Configuration.GetSection("RabbitMQ");
            string host = configSection["Host"];
            string userName = configSection["UserName"];
            string password = configSection["Password"];
            services.AddTransient<IMessagePublisher>((sp) => new RabbitMQMessagePublisher(host, userName, password, "Maap"));

            // add commandhandlers
            services.AddCommandHandlers();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "WorkshopManagement API", Version = "v1" });
            });

            services.AddHealthChecks(checks =>
            {
                checks.WithDefaultCacheDuration(TimeSpan.FromSeconds(1));
                checks.AddSqlCheck("EventStoreCN", Configuration.GetConnectionString("EventStoreCN"));
                checks.AddSqlCheck("WorkshopManagementCN", Configuration.GetConnectionString("WorkshopManagementCN"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IWorkshopPlanningRepository workshopPlanningRepo)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .Enrich.WithMachineName()
                .CreateLogger();

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            AutomapperConfigurator.SetupAutoMapper();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkshopManagement API - v1");
            });

            // initialize database
            workshopPlanningRepo.EnsureDatabase();
        }
    }
}
