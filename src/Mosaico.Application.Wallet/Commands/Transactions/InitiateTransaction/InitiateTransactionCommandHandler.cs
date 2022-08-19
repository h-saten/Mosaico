using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Commands.Transactions.InitiateNativeCurrencyPurchaseTransaction;
using Mosaico.Application.Wallet.Commands.Transactions.InitiateStableCoinPurchaseTransaction;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.Wallet.Exceptions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Transactions.InitiateTransaction
{
    public class InitiateTransactionCommandHandler : IRequestHandler<InitiateTransactionCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IWalletDbContext _walletDbContext;

        public InitiateTransactionCommandHandler(IWalletDbContext walletDbContext, IMediator mediator,
            ILogger logger = null)
        {
            _walletDbContext = walletDbContext;
            _mediator = mediator;
            _logger = logger;
        }

        public async Task<Guid> Handle(InitiateTransactionCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to initiate purchase transaction for {request.PaymentCurrency} currency");
            Guid transactionId;

            var paymentCurrency = await _walletDbContext
                .PaymentCurrencies
                .Where(x => x.Chain == request.Network && x.Ticker == request.PaymentCurrency)
                .SingleOrDefaultAsync(cancellationToken);

            if (paymentCurrency is null)
            {
                throw new UnsupportedPaymentCurrencyException(request.PaymentCurrency);
            }

            if (paymentCurrency.NativeChainCurrency)
            {
                transactionId = await _mediator.Send(new InitiateNativeCurrencyPurchaseTransactionCommand
                {
                    PaymentProcessor = request.PaymentProcessor,
                    TokenAmount = request.TokenAmount ?? 0,
                    ProjectId = request.ProjectId,
                    WalletAddress = request.WalletAddress,
                    PaymentCurrency = request.PaymentCurrency,
                    Network = request.Network
                }, cancellationToken);
            }
            else
            {
                transactionId = await _mediator.Send(new InitiateStableCoinPurchaseTransactionCommand
                {
                    PaymentProcessor = request.PaymentProcessor,
                    TokenAmount = request.TokenAmount ?? 0,
                    ProjectId = request.ProjectId,
                    WalletAddress = request.WalletAddress,
                    PaymentCurrency = request.PaymentCurrency,
                    Network = request.Network
                }, cancellationToken);
            }
            
            return transactionId;
        }
    }
}