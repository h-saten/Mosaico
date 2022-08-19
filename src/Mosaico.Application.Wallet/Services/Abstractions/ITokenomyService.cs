using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface ITokenomyService
    {
        Task<bool> ValidateAsync(Token token, CancellationToken t = new CancellationToken());
    }
}