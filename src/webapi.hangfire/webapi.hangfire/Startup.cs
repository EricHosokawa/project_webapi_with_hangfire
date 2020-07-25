using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using webapi.hangfire.Interfaces;
using webapi.hangfire.Models;
using webapi.hangfire.Repository;
using Swashbuckle.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using AutoMapper;
using webapi.hangfire.Entities;
using webapi.hangfire.App;
using webapi.hangfire.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Hangfire;
using MongoDB.Driver;
using MongoDB.Bson;

namespace webapi.hangfire
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
            //mongodb section
            services.Configure<MongoDbSetting>(Configuration.GetSection(nameof(MongoDbSetting)));
            services.AddSingleton<IMongoDbSetting>(m => m.GetRequiredService<IOptions<MongoDbSetting>>().Value);
            services.AddSingleton<MongoDbBase>();

            //context
            services.AddScoped<IEntityContext, EntityContext>();
            services.AddTransient<IEntityApplication, EntityApplication>();
            services.AddTransient<IJobsManager, JobManager>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "webapi hangfire v1",
                        Version = "v1",
                        Description = "webapi de teste com hanfire em Asp.NET Core 3.1"
                    });
            });

            services.AddAutoMapper(typeof(Startup));

            services.AddHealthChecks()
                .AddCheck<HealthCheck>("HealthCheck");

            services.AddControllers()
                .AddNewtonsoftJson(options => options.UseMemberCasing());

            var connectionString = Configuration.GetSection(nameof(MongoDbSetting))["ConnectionString"];
            var mongoUrlBuilder = new MongoUrlBuilder(connectionString);
            var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());

            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseMongoStorage(mongoClient, mongoUrlBuilder.DatabaseName, new MongoStorageOptions
                {
                    MigrationOptions = new MongoMigrationOptions
                    {
                        MigrationStrategy = new MigrateMongoMigrationStrategy(),
                        BackupStrategy = new CollectionMongoBackupStrategy()
                    },
                    Prefix = "hangfire.mongo",
                    CheckConnection = true
                }));

            services.AddHangfireServer(serverOptions =>
            {
                serverOptions.ServerName = "Hangfire.Mongo server 1";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, IBackgroundJobClient backgroundJobClient)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation($"{DateTime.Now} em ambiente de desenvolvimento (modo Debug).");
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("/swagger/v1/swagger.json", "webapi hangfire v1");
            });

            app.UseHealthChecks("/health");

            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
