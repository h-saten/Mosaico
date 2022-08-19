using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateTransaction
{
    public class UpdateTransactionCommandHandler : IRequestHandler<UpdateTransactionCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;

        public UpdateTransactionCommandHandler(IWalletDbContext walletDbContext, IProjectManagementClient projectManagementClient, IExchangeRateService exchangeRateService, ICrowdsalePurchaseService crowdsalePurchaseService)
        {
            _walletDbContext = walletDbContext;
            _projectManagementClient = projectManagementClient;
            _exchangeRateService = exchangeRateService;
            _crowdsalePurchaseService = crowdsalePurchaseService;
        }

        public async Task<Unit> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
        {
            var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == request.TransactionId,
                        CancellationToken.None);
            if(transaction == null || transaction.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed || 
                transaction.PaymentMethod != Domain.ProjectManagement.Constants.PaymentMethods.BankTransfer || !transaction.ProjectId.HasValue)
            {
                throw new TransactionNotFoundException("Transaction not found or is not supported for this operation");
            }
            
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(transaction.TokenId.ToString());
            }
            
            var project = await _projectManagementClient.GetProjectDetailsAsync(transaction.ProjectId.Value, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(transaction.ProjectId.Value);
            }
            
            var currentSaleStage = await _projectManagementClient.CurrentProjectSaleStage(project.Id, cancellationToken);
                
            if (currentSaleStage == null)
            {
                throw new ProjectStageNotExistException(project.Id);
            }
            
            var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(transaction.Currency);
            if (exchangeRate == null)
            {
                throw new InvalidExchangeRateException(transaction.Currency);
            }
            
            var estimatedTokenAmount = ((request.PayedAmount * exchangeRate.Rate) / currentSaleStage.TokenPrice);

            var canPurchase = await _crowdsalePurchaseService.CanPurchaseAsync(transaction.UserId, estimatedTokenAmount.Value, currentSaleStage.Id, 
                transaction.PaymentMethod);
            if (!canPurchase)
            {
                throw new UnauthorizedPurchaseException(transaction.UserId);
            }

            transaction.PayedAmount = request.PayedAmount;
            transaction.TokenAmount = estimatedTokenAmount;
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}