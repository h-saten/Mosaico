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
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers.Vault
{
    [EventInfo(nameof(CreateVaultDepositOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(CreateVaultDepositRequested))]
    public class CreateVaultDepositOnRequested : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IVaultv1Service _vaultService;
        private readonly ITokenService _tokenService;
        private readonly IDaoDispatcher _daoDispatcher;
        private readonly ILogger _logger;
        private readonly IOperationService _operationService;
        private readonly ICompanyWalletService _companyWalletService;

        public CreateVaultDepositOnRequested(IWalletDbContext walletDbContext, IVaultv1Service vaultService, IDaoDispatcher daoDispatcher, ILogger logger, ICompanyWalletService companyWalletService, ITokenService tokenService, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _vaultService = vaultService;
            _daoDispatcher = daoDispatcher;
            _logger = logger.ForContext(typeof(CreateVaultDepositRequested));
            _companyWalletService = companyWalletService;
            _tokenService = tokenService;
            _operationService = operationService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<CreateVaultDepositRequested>();
            if (data != null)
            {
                var distribution = await _walletDbContext.TokenDistributions.FirstOrDefaultAsync(t => t.Id == data.TokenDistributionId);
                if (distribution == null)
                {
                    _logger.Warning($"Token Distribution {data.TokenDistributionId} not found");
                    return;
                }
                    
                var operation = await _walletDbContext.Operations.OrderByDescending(o => o.CreatedAt)
                    .FirstOrDefaultAsync(t =>
                        t.Network == distribution.Token.Network && t.TransactionId == distribution.Id && t.Type == BlockchainOperationType.DEPOSIT_DEPLOYMENT);
            
                if (operation != null && (operation.State == OperationState.IN_PROGRESS ||
                                          operation.State == OperationState.SUCCESSFUL))
                {
                    throw new DepositIsDeployingException(distribution.Id);
                }
                
                operation ??= await _operationService.CreateVaultDepositDeploymentAsync(distribution.Token.Network, distribution.Id, data.UserId);
                await _operationService.SetTransactionInProgress(operation.Id, null);

                try
                {
                    var token = distribution.Token;
                    var vault = await _walletDbContext.Vaults.FirstOrDefaultAsync(v => v.TokenId == token.Id);

                    if (vault == null)
                    {
                        _logger.Warning($"Token {token.Symbol} does not has vault");
                        return;
                    }
                    var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(w => w.CompanyId == token.CompanyId && w.Network == token.Network);
                    if (companyWallet == null) throw new CompanyWalletNotFoundException(token.CompanyId, token.Network);
                    await _tokenService.SetWalletAllowanceAsync(token.Network, c =>
                    {
                        c.Amount = distribution.TokenAmount;
                        c.Decimals = 18;
                        c.ContractAddress = token.Address;
                        c.OwnerAddress = companyWallet.AccountAddress;
                        c.SpenderAddress = token.Vault.Address;
                        c.OwnerPrivateKey = companyWallet.PrivateKey;
                    });
                    var account = await _companyWalletService.GetAccountAsync(token.CompanyId, token.Network);
                    var depositResponse = await _vaultService.CreateDepositAsync(account, token.Network, c =>
                    {
                        c.Amount = distribution.TokenAmount;
                        c.Decimals = 18;
                        c.Recipient = companyWallet.AccountAddress;
                        c.TokenAddress = token.Address;
                        c.VaultAddress = token.Vault.Address;
                    });
                    await _operationService.SetTransactionOperationCompleted(operation.Id, hash: depositResponse.TransactionHash);
                    distribution.Vault = vault;
                    distribution.VaultId = vault.Id;
                    distribution.SmartContractId = depositResponse.Id;
                    _walletDbContext.TokenDistributions.Update(distribution);
                    await _walletDbContext.SaveChangesAsync();
                    await _daoDispatcher.DepositCreated(data.UserId);
                }
                catch (Exception ex)
                {
                    await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                    _logger.Error(ex, "Vault deployment failure");
                    await _daoDispatcher.DepositCreationFailed(data.UserId, ex.Message);
                }
            }
        }
    }
}