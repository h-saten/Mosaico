using System.Threading.Tasks;

namespace Mosaico.Events.Base
{
    public abstract class EventHandlerBase : IEventHandler
    {
        public abstract Task HandleAsync(CloudEvent @event);
    }
}