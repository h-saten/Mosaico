using System;
using System.Linq;
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
    [EventInfo(nameof(SendBankTransferDetailsOnCreated),  "wallets:api")]
    [EventTypeFilter(typeof(BankTransferTransactionInitiatedEvent))]   
    public class SendBankTransferDetailsOnCreated : EventHandlerBase
    {
        private readonly IWalletEmailService _walletEmailService;
        private readonly IUserManagementClient _userManagementClient;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IWalletDbContext _walletDbContext;

        public SendBankTransferDetailsOnCreated(IWalletEmailService walletEmailService, IWalletDbContext walletDbContext, IUserManagementClient userManagementClient, IProjectManagementClient projectManagementClient)
        {
            _walletEmailService = walletEmailService;
            _walletDbContext = walletDbContext;
            _userManagementClient = userManagementClient;
            _projectManagementClient = projectManagementClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<BankTransferTransactionInitiatedEvent>();
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
                            await _walletEmailService.SendBankTransferDetailsAsync(transaction, project, token, paymentDetails, user.Email, user.Language);
                        }
                    }
                }
            }
        }
    }
}