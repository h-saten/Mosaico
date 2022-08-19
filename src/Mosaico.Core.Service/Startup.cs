using System;
using System.Linq;
using AspNetCoreRateLimit;
using Autofac;
using MassTransit;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mosaico.API.Base.Configurations;
using Mosaico.API.Base.Extensions;
using Mosaico.API.Base.Filters;
using Mosaico.BackgroundJobs.Hangfire.Configurations;
using Mosaico.BackgroundJobs.Hangfire.Extensions;
using Mosaico.Base.Abstractions;
using Mosaico.Base.Extensions;
using Mosaico.Base.Settings;
using Mosaico.Cache.Redis.Configurations;
using Mosaico.Cache.Redis.Extensions;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Core.Service.Configurations;
using Mosaico.Integration.SignalR.Configurations;
using Mosaico.Integration.SignalR.Extensions;
using Mosaico.SDK.Identity.Configurations;
using Mosaico.Secrets.KeyVault.Configurations;
using Serilog;

namespace Mosaico.Core.Service
{
    public class Startup
    {
        private readonly HangfireConfig _hangfireConfiguration = new();
        private readonly LoggersSetting _loggersSettings = new();
        private readonly ServiceConfiguration _serviceConfiguration = new();
        private readonly IWebHostEnvironment _environment;
        private readonly KeyVaultConfiguration _keyVaultConfiguration = new();
        private readonly SqlServerConfiguration _sqlServerConfiguration = new();
        private readonly IdentityServerConfiguration _identityConfiguration = new();
        private readonly RedisCacheConfiguration _redisConfiguration = new();
        private readonly SignalrConfiguration _signalrConfiguration = new();

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration.GetSection(LoggersSetting.SectionName).Bind(_loggersSettings);
            Configuration.GetSection(SqlServerConfiguration.SectionName).Bind(_sqlServerConfiguration);
            Configuration.GetSection(ServiceConfiguration.SectionName).Bind(_serviceConfiguration);
            Configuration.GetSection(HangfireConfig.SectionName).Bind(_hangfireConfiguration);
            Configuration.GetSection(KeyVaultConfiguration.SectionName).Bind(_keyVaultConfiguration);
            Configuration.GetSection(IdentityServerConfiguration.SectionName).Bind(_identityConfiguration);
            Configuration.GetSection(RedisCacheConfiguration.SectionName).Bind(_redisConfiguration);
            Configuration.GetSection(SignalrConfiguration.SectionName).Bind(_signalrConfiguration);

            var certificate = services.GetCertificateAsync(_environment, _serviceConfiguration.Certificate, _keyVaultConfiguration).GetAwaiter().GetResult();
            //services.AddDataProtection(_sqlServerConfiguration.ConnectionString, "core", certificate);
            
            services.AddControllers(options => options.Filters.Add(typeof(APIExceptionFilterAttribute))).AddNewtonsoftJson();
            services.AddAPICors(_serviceConfiguration.AllowedOrigins);
            services.AddVersionedAPIWithSwagger();
            services.AddHttpContextAccessor();
            services.AddHangfire(_hangfireConfiguration);
            services.AddIdentityServerAuth(_identityConfiguration, _environment);
            services.AddClientIdentityServerAuth(_identityConfiguration, _environment);
            services.AddHttpClient();
            services.AddMassTransitHostedService(true);
            services.AddRateLimit(Configuration);
            services.AddRedisCache(_redisConfiguration);
            
            var signalRServerBuilder = services.AddSignalR();
            if (_signalrConfiguration.Provider == SignalrProviderType.Azure)
            {
                signalRServerBuilder.AddAzureSignalR(_signalrConfiguration.ConnectionString);
            }
            services.AddSingleton<ITelemetryInitializer, CoreTelemetryInitializer>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterInstance(Configuration).AsImplementedInterfaces();

            var logger = new LoggerConfiguration()
                .UseConsoleLogger(_loggersSettings.ConsoleLogger)
                .UseFileLogger(_loggersSettings.FileLogger)
                .UseApplicationInsights(_loggersSettings.ApplicationInsightsLogger)
                .MinimumLevel.Verbose()
                .CreateLogger();

            Log.Logger = logger;
            builder.RegisterInstance(logger).As<ILogger>();
            builder.AddUrl(_serviceConfiguration.BaseUri, "BaseUri");
            builder.RegisterModule(new ServiceModule(Configuration));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider,
            IMigrationRunner runner = null)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
                app.UseHttpsRedirection();
            }

            if (_serviceConfiguration.SwaggerEnabled)
            {
                // http://localhost:10000/core/swagger
                // http://localhost:5001/core/swagger
                
                app.UseSwagger(options =>
                {
                    options.RouteTemplate = "core/swagger/{documentName}/swagger.json";
                });
                app.UseSwaggerUI(
                    options =>
                    {
                        // build a swagger endpoint for each discovered API version
                        foreach (var description in provider.ApiVersionDescriptions)
                        {
                            options.SwaggerEndpoint($"/core/swagger/{description.GroupName}/swagger.json",
                                description.GroupName.ToUpperInvariant());
                            options.RoutePrefix = "core/swagger";
                        }
                    });
            }
            
            app.UseCors();
            app.AddHeaderPolicies();
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto |
                                   ForwardedHeaders.XForwardedHost
            });
            
            app.UseRouting();
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseIpRateLimiting();
            app.UseHangfire(_hangfireConfiguration);
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.AddCoreHubs(_signalrConfiguration);
            });
            
            if (_serviceConfiguration.RunMigrations)
            {
                if (runner == null) throw new Exception("MigrationRunner was not registered");
                runner.RunMigrations();
            }
        }
    }
}