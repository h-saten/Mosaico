using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Transactions.ConfirmPurchaseTransaction
{
    public class ConfirmPurchaseTransactionCommandHandler : IRequestHandler<ConfirmPurchaseTransactionCommand>
    {
        private readonly IProjectDbContext _projectDb;
        private readonly ILogger _logger;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWalletDbContext _walletDbContext;

        public ConfirmPurchaseTransactionCommandHandler(IProjectDbContext projectDb, IEventFactory eventFactory, IEventPublisher eventPublisher, IWalletDbContext walletDbContext, ILogger logger = null)
        {
            _projectDb = projectDb;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _walletDbContext = walletDbContext;
            _logger = logger;
        }

        public async Task<Unit> Handle(ConfirmPurchaseTransactionCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _projectDb.BeginTransaction())
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

                    var project = await _projectDb.Projects.FirstOrDefaultAsync(p => p.TokenId == transac.TokenId, cancellationToken);
                    if (project == null)
                    {
                        throw new ProjectNotFoundException($"related to token {transac.TokenId}");
                    }
                    
                    _logger?.Verbose($"Project {project.Id} exists");
                    
                    ValidateTransaction(request, project, transac);
                    _logger?.Verbose($"Transaction is valid");

                    var status = await _walletDbContext.TransactionStatuses.FirstOrDefaultAsync(t => t.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed, cancellationToken);
    
                    transac.SetStatus(status);
                    transac.Currency = request.Currency;
                    transac.PayedAmount = request.PayedAmount;
                    transac.FinishedAt = DateTimeOffset.UtcNow;

                    _logger?.Verbose($"Updating transaction");
                    
                    await _walletDbContext.SaveChangesAsync(cancellationToken);
                    
                    _logger?.Verbose($"Sending events to Redis and Service Bus");
                    await PublishEventsAsync(request, cancellationToken);

                    await transaction.CommitAsync(cancellationToken);
                    _logger?.Verbose($"Finalized transaction");
                    return Unit.Value;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
        }

        private static void ValidateTransaction(ConfirmPurchaseTransactionCommand request, Project project, Transaction transac)
        {
            if (project.Status.Key != Domain.ProjectManagement.Constants.ProjectStatuses.InProgress)
            {
                throw new InvalidProjectStatusException(project.Id.ToString());
            }

            if (transac.Status.Key != Domain.Wallet.Constants.TransactionStatuses.Pending)
            {
                throw new InvalidTransactionStatusException(request.TransactionId.ToString());
            }
        }

        private async Task PublishEventsAsync(ConfirmPurchaseTransactionCommand request, CancellationToken cancellationToken)
        {
            var cloudEvent = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new TransactionConfirmedEvent
                {
                    Currency = request.Currency,
                    Payed = request.PayedAmount,
                    TransactionId = request.TransactionId.Value
                });
            await _eventPublisher.PublishAsync(cloudEvent.Source, cloudEvent, cancellationToken);
        }
    }
}