using System;
using Autofac;
using Hangfire;
using Hangfire.SqlServer;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Mosaico.BackgroundJobs.Hangfire.Configurations;
using Mosaico.Base.Exceptions;

namespace Mosaico.BackgroundJobs.Hangfire.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void AddHangfire(this IServiceCollection services, HangfireConfig hangfireConfig, bool enableServer = true)
        {
            if (hangfireConfig?.IsEnabled == true)
            {
                Console.WriteLine(hangfireConfig.ConnectionString);
                services.AddHangfire(configuration =>
                {
                    configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSqlServerStorage(hangfireConfig.ConnectionString, new SqlServerStorageOptions
                        {
                            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                            CommandTimeout = TimeSpan.FromSeconds(120),
                            QueuePollInterval = TimeSpan.Zero,
                            UseRecommendedIsolationLevel = true,
                            DisableGlobalLocks = true,
                            SchemaName = hangfireConfig.DatabaseSchema
                        });
                });
                if (enableServer)
                {
                    services.AddHangfireServer();
                }
            }
            else
            {
                throw new InvalidConfigException("Hangfire");
            }
        }

        public static void UseHangfire(this IApplicationBuilder app, HangfireConfig hangfireConfig)
        {
            if (hangfireConfig?.IsEnabled == true && hangfireConfig?.IsDashboardEnabled == true)
            {
                app.UseHangfireDashboard($"/{hangfireConfig.DashboardUrl}", new DashboardOptions
                {
                    DashboardTitle = "Mosaico Hangfire Dashboard",
                    DisplayStorageConnectionString = false,
                    PrefixPath = hangfireConfig.DashboardPrefix,
                    Authorization = new[]
                    {
                        new HangfireCustomBasicAuthenticationFilter
                        {
                            User = "mosaico",
                            Pass = hangfireConfig.AccessPassword
                        }
                    }
                });
            }
        }
    }
}