using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;

namespace Mosaico.Application.Wallet.EventHandlers.Vault
{
    [EventInfo(nameof(SendVaultTokensOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(VaultSendRequested))]
    public class SendVaultTokensOnRequested : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IVaultv1Service _vaultv1Service;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly IDaoDispatcher _daoDispatcher;
        private readonly IUserWalletService _userWalletService;
        private readonly IOperationService _operationService;

        public SendVaultTokensOnRequested(IWalletDbContext walletDbContext, IVaultv1Service vaultv1Service, ICompanyWalletService companyWalletService, IDaoDispatcher daoDispatcher, IUserWalletService userWalletService, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _vaultv1Service = vaultv1Service;
            _companyWalletService = companyWalletService;
            _daoDispatcher = daoDispatcher;
            _userWalletService = userWalletService;
            _operationService = operationService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<VaultSendRequested>();
            if (data != null)
            {
                var distribution = await _walletDbContext.TokenDistributions.FirstOrDefaultAsync(d => d.Id == data.TokenDistributionId);
                if (distribution == null) throw new TokenDistributionNotFoundException(data.TokenDistributionId.ToString());
                var token = distribution.Token;
                if (token == null) throw new TokenNotFoundException(distribution.TokenId);
                
                var vault = await _walletDbContext.Vaults.FirstOrDefaultAsync(v => v.TokenId == token.Id);
                
                if (vault == null)
                    throw new VaultNotFoundException(Guid.Empty);
                    
                var operation = await _walletDbContext.Operations.OrderByDescending(o => o.CreatedAt)
                    .FirstOrDefaultAsync(t => 
                        t.Network == vault.Token.Network && t.TransactionId == vault.Id && t.Type == BlockchainOperationType.VAULT_TRANSFER);
            
                if (operation != null && operation.State == OperationState.IN_PROGRESS)
                {
                    throw new VaultTransferInProgressException(vault.Id);
                }
                
                operation ??= await _operationService.CreateVaultTransferOperationAsync(token.Network, vault.Id, data.UserId);
                await _operationService.SetTransactionInProgress(operation.Id, null);
                
                try
                {
                    var account = await _companyWalletService.GetAccountAsync(token.CompanyId, token.Network);
                    var response = await _vaultv1Service.SendAsync(account, token.Network, c =>
                    {
                        c.Amount = data.Amount;
                        c.Id = distribution.SmartContractId;
                        c.Recipient = data.Recipient;
                        c.VaultAddress = vault.Address;
                    });
                    if (response.Status == 1)
                    {
                        await _operationService.SetTransactionOperationCompleted(operation.Id, hash: response.TransactionHash);
                    }
                    else
                    {
                        throw new Exception($"Transaction failed on blockchain");
                    }

                    await _daoDispatcher.VaultSent(data.UserId);
                    await _userWalletService.AddTokenToWalletAsync(data.Recipient, token.Address, token.Network);
                    await _companyWalletService.AddTokenToWalletAsync(data.Recipient, token.Address, token.Network);
                }
                catch (Exception e)
                {
                    await _operationService.SetTransactionOperationFailed(operation.Id, e.Message);
                    await _daoDispatcher.VaultSendFailed(data.UserId, e.Message);
                    throw;
                }
            }
        }
    }
}