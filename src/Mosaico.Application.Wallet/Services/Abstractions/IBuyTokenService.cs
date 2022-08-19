using System;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IBuyTokenService
    {
        Task BuyAsync(Guid transactionId, CancellationToken cancellationToken);
        Task<decimal> GetExchangeRateAsync(string ticker, decimal tokenPrice);
    }
}