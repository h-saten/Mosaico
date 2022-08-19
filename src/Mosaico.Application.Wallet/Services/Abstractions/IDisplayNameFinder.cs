using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IDisplayNameFinder
    {
        public Task<string> FindDisplayNameAsync(string walletAddress, CancellationToken token = new CancellationToken());
    }
}