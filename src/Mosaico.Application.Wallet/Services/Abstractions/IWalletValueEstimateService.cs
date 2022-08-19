using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IWalletValueEstimateService
    {
        Task<decimal> GetWalletValueAsync(Guid companyWalletId,
            string paymentCurrency = Domain.Wallet.Constants.PaymentCurrencies.USDT,
            CancellationToken cancellationToken = new CancellationToken());
    }
}