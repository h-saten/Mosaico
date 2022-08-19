using System.Reflection;
using Autofac;
using Mosaico.BackgroundJobs.Base;

namespace Mosaico.BackgroundJobs.Hangfire.Extensions
{
    public static class ContainerBuilderExtensions
    {
        public static void RegisterHangfireJob<TJob>(this ContainerBuilder builder) where TJob : IBackgroundJob
        {
            builder.RegisterType<TJob>().AsSelf().As<IBackgroundJob>();
            if (typeof(TJob).GetCustomAttribute(typeof(BackgroundJobAttribute)) is BackgroundJobAttribute attr)
            {
                builder.RegisterType<TJob>().Keyed<IBackgroundJob>(attr.Name);
            }
        }
    }
}