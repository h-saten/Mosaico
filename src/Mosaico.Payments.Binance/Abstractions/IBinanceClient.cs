using System.Threading;
using System.Threading.Tasks;
using Mosaico.Payments.Binance.Models;

namespace Mosaico.Payments.Binance.Abstractions
{
    public interface IBinanceClient
    {
        Task<BinanceOrderResponse> GetOrderAsync(BinanceOrderRequest payload, CancellationToken token = new CancellationToken());
        Task<BinanceOrderCreationResponse> CreateOrderAsync(BinanceOrderCreationRequest payload, CancellationToken token = new CancellationToken());
    }
}