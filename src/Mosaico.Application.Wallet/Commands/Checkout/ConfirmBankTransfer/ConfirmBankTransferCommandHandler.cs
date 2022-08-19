using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Checkout.ConfirmBankTransfer
{
    public class ConfirmBankTransferCommandHandler : IRequestHandler<ConfirmBankTransferCommand>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICrowdsalePurchaseService _crowdsaleService;
        private readonly IOperationService _operationService;
        private readonly ILogger _logger;
        
        public ConfirmBankTransferCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, IWalletDbContext walletDbContext, ICrowdsalePurchaseService crowdsaleService, IOperationService operationService,ILogger logger)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _walletDbContext = walletDbContext;
            _crowdsaleService = crowdsaleService;
            _operationService = operationService;
            _logger = logger;
        }

        public async Task<Unit> Handle(ConfirmBankTransferCommand request, CancellationToken cancellationToken)
        {
            
            var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.TransactionId, cancellationToken: cancellationToken);

            if (transaction != null &&
                transaction.PaymentProcessor == Domain.ProjectManagement.Constants.PaymentMethods.BankTransfer &&
                (transaction.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Pending ||
                 transaction.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Expired))
            {

                var operation = await _operationService.GetLatestOperationAsync(transaction.Id);
                if (operation != null && operation.State != OperationState.FAILED)
                    throw new OtherOperationInProgressException(transaction.Id);

                var statuses =
                    await _walletDbContext.TransactionStatuses.ToListAsync(cancellationToken: cancellationToken);

                var canPurchase = await _crowdsaleService.CanPurchaseAsync(transaction.UserId,
                    transaction.TokenAmount.Value,
                    transaction.StageId.Value, Domain.ProjectManagement.Constants.PaymentMethods.BankTransfer, true);

                if (canPurchase)
                {
                    await _crowdsaleService.AddTokenToUserWalletAsync(transaction.TokenId.Value, transaction.To,
                        transaction.Network);
                    var transactionHash = await _crowdsaleService.TransferTargetTokensToUserAsync(transaction);
                    transaction.TransactionHash = transactionHash;
                    await _operationService.CreateTransactionOperationAsync(transaction.Network, transaction.Id,
                        transactionHash, projectId: transaction.ProjectId);
                }
                else
                {
                    transaction.SetStatus(statuses.FirstOrDefault(s =>
                        s.Key == Domain.Wallet.Constants.TransactionStatuses.Failed));
                    transaction.FailureReason = "The crowdsale is already closed";
                    await PublishFailEventAsync(transaction);
                    throw new PurchaseFailedException(transaction.FailureReason);
                }

                _walletDbContext.Transactions.Update(transaction);
                await _walletDbContext.SaveChangesAsync(cancellationToken);

                return Unit.Value;
            }
            throw new TransactionNotFoundException(request.TransactionId.ToString());
        }
        
        private async Task PublishFailEventAsync(Transaction transaction)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new PurchaseFailedEvent(transaction.UserId, transaction.TokenAmount.Value,
                    transaction.StageId.Value, transaction.Id));
            await _eventPublisher.PublishAsync(e);
        }
    }
}