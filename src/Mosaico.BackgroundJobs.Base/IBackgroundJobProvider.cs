using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Mosaico.BackgroundJobs.Base
{
    public interface IBackgroundJobProvider
    {
        string Add(string jobName, DateTimeOffset runAt, TimeZoneInfo timeZoneInfo, string queue = "default", object parameters = null);
        string Add(IBackgroundJob job, TimeZoneInfo timeZone, string queue = "default", object parameters = null);
        string Execute(Expression<Func<Task>> job);
        string Add(string cron, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default");
        string Add(string id, string cron, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue = "default");
        void Remove(string id);
        void RemoveAllRecurrentJobs();
        void Trigger(string id);
        string Add(DateTimeOffset runAt, IBackgroundJob job, TimeZoneInfo timeZone, string queue = "default", object parameters = null);
        string Add(DateTimeOffset runAt, Expression<Func<Task>> job, TimeZoneInfo timeZone, string queue= "default");
    }
}