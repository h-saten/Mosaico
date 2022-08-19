using System.Threading.Tasks;
using Hangfire.Server;
using Mosaico.BackgroundJobs.Base;

namespace Mosaico.BackgroundJobs.Hangfire.Abstractions
{
    public abstract class HangfireBackgroundJobBase : IBackgroundJob
    {
        protected PerformContext JobContext;
        protected string JobName;
        
        public Task ExecuteJobAsync(string name, PerformContext context, object parameters = null)
        {
            JobContext = context;
            JobName = name;
            return ExecuteAsync(parameters);
        }

        public abstract Task ExecuteAsync(object parameters = null);
    }
}