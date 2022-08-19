using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(SendEmailOnSuccessfulPurchase),  "wallets:api")]
    [EventTypeFilter(typeof(SuccessfulPurchaseEvent))]  
    public class SendEmailOnSuccessfulPurchase : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IWalletEmailService _walletEmailService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IUserManagementClient _userManagementClient;
        
        public SendEmailOnSuccessfulPurchase(IWalletDbContext walletDbContext, IWalletEmailService walletEmailService, IProjectManagementClient projectManagementClient, IUserManagementClient userManagementClient)
        {
            _walletDbContext = walletDbContext;
            _walletEmailService = walletEmailService;
            _projectManagementClient = projectManagementClient;
            _userManagementClient = userManagementClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<SuccessfulPurchaseEvent>();
            if(data != null)
            {
                var transaction = await _walletDbContext.Transactions.FirstOrDefaultAsync(t => t.Id == data.TransactionId);
                if (transaction != null && transaction.StageId.HasValue)
                {
                    var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == transaction.TokenId.Value);
                    var stage = await _projectManagementClient.GetStageAsync(transaction.StageId.Value);
                    var project = await _projectManagementClient.GetProjectAsync(stage?.ProjectId ?? Guid.Empty);
                    
                    var paymentDetails = await _walletDbContext.ProjectBankTransferTitles
                        .Include(p => p.ProjectBankPaymentDetails)
                        .FirstOrDefaultAsync(t => t.Reference == transaction.CorrelationId);
                    
                    if(paymentDetails != null && project != null)
                    {
                        var user = await _userManagementClient.GetUserAsync(data.UserId);
                        if (!string.IsNullOrWhiteSpace(user?.Email))
                        {
                            await _walletEmailService.SendTransactionFinishedAsync(transaction, project, token, user.Email, user.Language);
                        }
                    }
                }
            }
        }
    }
}