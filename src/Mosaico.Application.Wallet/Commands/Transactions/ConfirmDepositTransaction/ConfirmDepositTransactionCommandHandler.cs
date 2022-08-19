using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.EventSourcing.Base;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Transactions.ConfirmDepositTransaction
{
    public class ConfirmDepositTransactionCommandHandler : IRequestHandler<ConfirmDepositTransactionCommand>
    {
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWalletDbContext _walletDbContext;
        
        public ConfirmDepositTransactionCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, ISystemEventFactory systemEventFactory, IWalletDbContext walletDbContext, ILogger logger = null)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _walletDbContext = walletDbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(ConfirmDepositTransactionCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _walletDbContext.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Preparing to confirm transaction {request.TransactionId}");
                    
                    var transac = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken);
                    if (transac == null)
                    {
                        throw new TransactionNotFoundException(request.TransactionId.ToString());
                    }
                    _logger?.Verbose($"Transaction {transac.Id} exists");
                    ValidateTransaction(transac);
                    _logger?.Verbose($"Transaction is valid");
                    var status = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed, cancellationToken);
                    if (status == null)
                    {
                        throw new InvalidTransactionStatusException(Domain.Wallet.Constants.TransactionStatuses.Confirmed);
                    }
                    
                    transac.SetStatus(status);
                    transac.Currency = request.Currency;
                    transac.PayedAmount = request.PayedAmount;
                    transac.FinishedAt = DateTimeOffset.UtcNow;
                    //TODO: also align with blockchain balance
                    
                    _logger?.Verbose($"Updating transaction");
                    await _walletDbContext.SaveChangesAsync(cancellationToken);
                    
                    _logger?.Verbose($"Sending events to Redis and Service Bus");
                    await PublishEventsAsync(request, cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private void ValidateTransaction(Transaction transac)
        {
            if (transac.Type.Key != Domain.Wallet.Constants.TransactionType.Deposit)
            {
                throw new InvalidTransactionTypeException(transac.Type.Key);
            }
        }

        private async Task PublishEventsAsync(ConfirmDepositTransactionCommand request, CancellationToken cancellationToken)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new TransactionConfirmedEvent
                {
                    Payed = request.PayedAmount,
                    TransactionId = request.TransactionId.Value,
                    Currency = request.Currency
                });
            await _eventPublisher.PublishAsync(cloudEvent.Source, cloudEvent, cancellationToken);
        }
    }
}