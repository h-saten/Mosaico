using System.Threading.Tasks;

namespace Mosaico.Events.Base
{
    public interface IEventHandler
    {
        Task HandleAsync(CloudEvent @event);
    }
}