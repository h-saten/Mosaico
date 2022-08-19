using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.SDK.Features.Abstractions
{
    public interface IFeatureGuard
    {
        Task<bool> CanExecuteAsync(string name, string userId, CancellationToken t = new CancellationToken());
    }
}