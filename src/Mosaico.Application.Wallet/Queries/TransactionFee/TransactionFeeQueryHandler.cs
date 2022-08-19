using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Queries.TransactionFee
{
    public class TransactionFeeQueryHandler : IRequestHandler<TransactionFeeQuery, TransactionFeeQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;

        public TransactionFeeQueryHandler(IWalletDbContext walletDbContext, IExchangeRateService exchangeRateService)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<TransactionFeeQueryResponse> Handle(TransactionFeeQuery request, CancellationToken cancellationToken)
        {
            var transactionsQuery = _walletDbContext.Transactions.Where(t =>
                t.ProjectId == request.ProjectId && t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase &&
                t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed && t.PayedAmount > 0 && t.PayedInUSD > 0 && t.MosaicoFeeInUSD > 0);

            var feeResponse = await CalculateFeeAsync(transactionsQuery, cancellationToken);
            return feeResponse;
        }

        private async Task<TransactionFeeQueryResponse> CalculateFeeAsync(IQueryable<Transaction> transactionsQuery, CancellationToken cancellationToken)
        {
            var transactions = await transactionsQuery.ToListAsync(cancellationToken);
            var totalMosacioFee = 0m;
            var paymentMethodFee = new Dictionary<string, decimal>();
            foreach (var transaction in transactions)
            {
                var mosaicoFee = transaction.MosaicoFeeInUSD.Value;
                totalMosacioFee += mosaicoFee;
                if(!paymentMethodFee.ContainsKey(transaction.PaymentProcessor))
                {
                    paymentMethodFee.Add(transaction.PaymentProcessor, mosaicoFee);
                }
                else
                {
                    paymentMethodFee[transaction.PaymentProcessor] += mosaicoFee;
                }
            }

            return new TransactionFeeQueryResponse
            {
                TotalAmount = totalMosacioFee,
                Currency = Constants.FIATCurrencies.USD,
                TransactionCount = transactions.Count,
                Fees = paymentMethodFee
            };;

        }
    }
}