using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hangfire;
using Mosaico.BackgroundJobs.Base;

namespace Mosaico.Application.Wallet.Tests.EventHandlers
{
    public class BackgroundJobProviderStub : IBackgroundJobProvider
    {
        public string Add(string jobName, DateTimeOffset runAt, TimeZoneInfo timeZoneInfo, string queue = "default",
            object parameters = null)
        {
            throw new NotImplementedException();
        }

        public string Add(IBackgroundJob job, TimeZoneInfo timeZone, string queue = "default", object parameters = null)
        {
            throw new NotImplementedException();
        }

        public string Execute(Expression<Func<Task>> job)
        {
            return BackgroundJob.Enqueue(job);
        }

        public string Add(string cron, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default")
        {
            throw new NotImplementedException();
        }

        public string Add(string id, string cron, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default")
        {
            throw new NotImplementedException();
        }

        public void Remove(string id)
        {
            throw new NotImplementedException();
        }

        public void RemoveAllRecurrentJobs()
        {
            throw new NotImplementedException();
        }

        public void Trigger(string id)
        {
            throw new NotImplementedException();
        }

        public string Add(DateTimeOffset runAt, IBackgroundJob job, TimeZoneInfo timeZone, string queue = "default",
            object parameters = null)
        {
            throw new NotImplementedException();
        }

        public string Add(DateTimeOffset runAt, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default")
        {
            throw new NotImplementedException();
        }
    }
}