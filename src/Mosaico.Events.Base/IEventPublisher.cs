using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Events.Base
{
    public interface IEventPublisher
    {
        Task PublishAsync([NotNull] string path, [NotNull] CloudEvent payload, CancellationToken t);
        Task PublishAsync(string path, CloudEvent payload);
        Task PublishAsync(CloudEvent payload);
        Task SendAsync([NotNull] string path, [NotNull] CloudEvent payload, CancellationToken t);
    }
}