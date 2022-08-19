using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.BackgroundJobs.Hangfire.Configurations;
using Module = Autofac.Module;

namespace Mosaico.BackgroundJobs.Hangfire
{
    public class HangfireModule : Module
    {
        private readonly HangfireConfig _configuration = new();

        public HangfireModule(IConfiguration configuration)
        {
            configuration.GetSection(HangfireConfig.SectionName).Bind(_configuration);
        }

        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterInstance(_configuration).AsSelf();
            if (_configuration?.IsEnabled == true)
            {
                builder.RegisterType<HangfireJobProvider>().As<IBackgroundJobProvider>();
                builder.RegisterBuildCallback(RegisterJobs);
            }
        }

        private void RegisterJobs(ILifetimeScope scope)
        {
            GlobalConfiguration.Configuration.UseAutofacActivator(scope);
            var provider = scope.Resolve<IBackgroundJobProvider>();
            provider.RemoveAllRecurrentJobs();
            var jobs = scope.ResolveOptional<IEnumerable<IBackgroundJob>>();
            if (jobs != null)
            {
                foreach (var job in jobs)
                {
                    if (job is HangfireBackgroundJobBase hangfireJob)
                    {
                        if (hangfireJob.GetType().GetCustomAttribute(typeof(BackgroundJobAttribute)) is BackgroundJobAttribute
                            attribute && !string.IsNullOrWhiteSpace(attribute.Name) && attribute.IsRecurring)
                        {
                            attribute.Queue = string.IsNullOrWhiteSpace(attribute.Queue) ? "default" : attribute.Queue;
                            var cron = attribute.Cron;
                            if (_configuration.JobSchedule != null &&
                                _configuration.JobSchedule.ContainsKey(attribute.Name))
                            {
                                cron = _configuration.JobSchedule[attribute.Name];
                            }

                            if (!string.IsNullOrWhiteSpace(cron))
                            {
                                provider.Remove(attribute.Name);
                                provider.Add(attribute.Name, cron,
                                    () => hangfireJob.ExecuteJobAsync(attribute.Name, null, null), TimeZoneInfo.Utc,
                                    attribute.Queue);
                            }

                            if (attribute.ExecutedOnStartup)
                            {
                                provider.Execute(() => hangfireJob.ExecuteAsync(null));
                            }
                        }
                    }
                }
            }
        }
    }
}