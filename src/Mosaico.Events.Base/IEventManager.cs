using System.Threading.Tasks;

namespace Mosaico.Events.Base
{
    public interface IEventManager
    {
        Task RegisterListenerAsync();
    }
}