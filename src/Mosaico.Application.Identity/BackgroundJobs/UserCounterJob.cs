using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.BackgroundJobs.Base;
using Mosaico.BackgroundJobs.Hangfire.Abstractions;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;

namespace Mosaico.Application.Identity.BackgroundJobs
{
    [BackgroundJob("user-counter-job", IsRecurring = true, Cron = "0 0 */1 * *", ExecutedOnStartup = true)]
    public class UserCounterJob : HangfireBackgroundJobBase
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IIdentityContext _identityContext;
        
        public UserCounterJob(IEventFactory eventFactory, IEventPublisher eventPublisher, IIdentityContext identityContext)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _identityContext = identityContext;
        }

        public override async Task ExecuteAsync(object parameters = null)
        {
            var count = await _identityContext.Users.LongCountAsync();
            var e = _eventFactory.CreateEvent(Events.Identity.Constants.EventPaths.Users, new UserCountUpdated(count));
            await _eventPublisher.PublishAsync(e);
        }
    }
}