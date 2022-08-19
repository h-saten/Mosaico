using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.BackgroundJobs.Hangfire.Exceptions;

namespace Mosaico.BackgroundJobs.Hangfire
{
    public class HangfireJobProvider : IBackgroundJobProvider
    {
        private readonly IBackgroundJobClient _client;
        private readonly IRecurringJobManager _recurringJobManager;
        private readonly JobStorage _jobStorage;
        private readonly IIndex<string, IBackgroundJob> _jobs;

        public HangfireJobProvider(IBackgroundJobClient client, IRecurringJobManager recurringJobManager, JobStorage jobStorage, IIndex<string, IBackgroundJob> jobs = null)
        {
            _client = client;
            _recurringJobManager = recurringJobManager;
            _jobStorage = jobStorage;
            _jobs = jobs;
        }

        public string Add(string jobName, DateTimeOffset runAt, TimeZoneInfo timeZoneInfo, string queue = "default", object parameters = null)
        {
            if (_jobs != null && _jobs.TryGetValue(jobName, out var job))
            {
                if (job is HangfireBackgroundJobBase hangfireJob)
                {
                    return _client.Schedule(() => hangfireJob.ExecuteJobAsync(jobName, null, parameters), runAt);;
                }
            }

            throw new InvalidHangfireJobException(jobName);
        }

        public string Add(IBackgroundJob job, TimeZoneInfo timeZone, string queue = "default", object parameters = null)
        {
            if (job is HangfireBackgroundJobBase hangfireJob && job.GetType().GetCustomAttribute(typeof(BackgroundJobAttribute)) is BackgroundJobAttribute attr)
            {
                return Add(attr.Cron, () => hangfireJob.ExecuteJobAsync(attr.Name, null, parameters), timeZone, queue);
            }

            throw new InvalidHangfireJobException("Background job registration");
        }

        public string Execute(Expression<Func<Task>> job)
        {
            return _client.Enqueue(job);
        }
        
        public string Add(string cron, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default")
        {
            var id = Guid.NewGuid().ToString();
            _recurringJobManager.AddOrUpdate(id, job, cron, timeZone, queue);
            return id;
        }
        
        public string Add(string id, string cron, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default")
        {
            _recurringJobManager.AddOrUpdate(id, job, cron, timeZone, queue);
            return id;
        }

        public void Remove(string id)
        {
            _recurringJobManager.RemoveIfExists(id);
            _client.Delete(id);
        }

        public void Trigger(string id)
        {
            _recurringJobManager.Trigger(id);
        }

        public string Add(DateTimeOffset runAt, IBackgroundJob job, TimeZoneInfo timeZone, string queue = "default", object parameters = null)
        {
            if (job is HangfireBackgroundJobBase hangfireJob && job.GetType().GetCustomAttribute(typeof(BackgroundJobAttribute)) is BackgroundJobAttribute attr)
            {
                return _client.Schedule(() => hangfireJob.ExecuteJobAsync(attr.Name, null, parameters), runAt);
            }
            
            throw new InvalidHangfireJobException("Background job registration");
        }

        public string Add(DateTimeOffset runAt, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default")
        {
            return _client.Schedule(job, runAt);
        }

        public void RemoveAllRecurrentJobs()
        {
            var connection = _jobStorage.GetConnection();
            foreach (var recurringJob in connection.GetRecurringJobs())
            {
                _recurringJobManager.RemoveIfExists(recurringJob.Id);
            }
        }
    }
}