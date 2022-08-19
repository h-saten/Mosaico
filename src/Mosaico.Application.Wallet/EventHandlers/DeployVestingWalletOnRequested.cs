using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(DeployVestingWalletOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(VestingDeploymentRequested))]
    public class DeployVestingWalletOnRequested : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IVaultv1Service _vaultv1Service;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IDaoDispatcher _daoDispatcher;
        private readonly IOperationService _operationService;

        public DeployVestingWalletOnRequested(IWalletDbContext walletDbContext, IVaultv1Service vaultv1Service, ILogger logger, ICompanyWalletService companyWalletService, IDaoDispatcher daoDispatcher, ITokenService tokenService, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _vaultv1Service = vaultv1Service;
            _companyWalletService = companyWalletService;
            _daoDispatcher = daoDispatcher;
            _tokenService = tokenService;
            _operationService = operationService;
            _logger = logger.ForContext(typeof(DeployVestingWalletOnRequested));
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<VestingDeploymentRequested>();
            if (data != null)
            {
                var vesting = await _walletDbContext.Vestings.FirstOrDefaultAsync(v => v.Id == data.VestingId);
                if (vesting == null)
                {
                    _logger.Warning($"Vesting {data.VestingId} not found");
                    return;
                }
                
                var token = vesting.Token;
                var vault = await _walletDbContext.Vaults.FirstOrDefaultAsync(v => v.TokenId == token.Id);
                if (vault == null)
                {
                    _logger?.Warning($"Vault was not deployed for token {token.Id}");
                    return;
                }
                
                var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o =>
                    o.Network == vault.Network && o.Type == BlockchainOperationType.VESTING_CREATION &&
                    o.TransactionId == vesting.Id);
                
                if (operation != null && operation.State == OperationState.IN_PROGRESS)
                {
                    throw new VestingCreationIsInProgressException(vault.Id);
                }

                operation ??= await _operationService.CreateVestingDeploymentTransaction(vault.Network, vesting.Id, vault.Address, data.UserId);
                await _operationService.SetTransactionInProgress(operation.Id, null);
                
                try
                {
                    var companyWalletAccount = await _companyWalletService.GetAccountAsync(token.CompanyId, token.Network);
                    if (companyWalletAccount == null)
                    {
                        _logger?.Warning($"Company wallet for token {token.Id} within network {token.Network} was not found");
                        return;
                    }
                    
                    var totalTokenApproval = vesting.Funds.Where(f => f.Status == VestingFundStatus.Pending || f.Status == VestingFundStatus.Failed).Sum(f => f.TokenAmount);
                    var approvalTransaction = await _tokenService.ApproveAsync(token.Network, companyWalletAccount, token.Address, vault.Address, totalTokenApproval);
                    _logger.Information($"Successfully approved {totalTokenApproval} {token.Symbol} for vault {vault.Address} - {approvalTransaction}");
                    
                    foreach (var fund in vesting.Funds)
                    {
                        if (fund.Status == VestingFundStatus.Pending || fund.Status == VestingFundStatus.Failed)
                        {
                            fund.Status = VestingFundStatus.Deploying;
                            fund.ApprovalTransactionHash = approvalTransaction;
                            _walletDbContext.VestingFunds.Update(fund);
                            await _walletDbContext.SaveChangesAsync();
                            try
                            {
                                var depositResponse = await _vaultv1Service.CreateDepositAsync(companyWalletAccount,
                                    token.Network, c =>
                                    {
                                        c.Amount = fund.TokenAmount;
                                        c.Decimals = 18;
                                        c.Recipient = vesting.WalletAddress;
                                        c.TokenAddress = token.Address;
                                        c.VaultAddress = vault.Address;
                                        c.AvailableAt = fund.StartAt ?? DateTimeOffset.UtcNow;
                                    });
                                _logger.Information(
                                    $"Successfully created vesting fund {depositResponse.Id} in {vault.Address}");
                                fund.SmartContractId = depositResponse.Id;
                                fund.Status = VestingFundStatus.Deployed;
                                fund.TransactionHash = depositResponse.TransactionHash;
                                _walletDbContext.Operations.Update(operation);
                                _walletDbContext.VestingFunds.Update(fund);
                                await _walletDbContext.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                fund.Status = VestingFundStatus.Failed;
                                fund.FailureReason = ex.Message;
                                _walletDbContext.VestingFunds.Update(fund);
                                await _walletDbContext.SaveChangesAsync();
                                throw;
                            }
                        }
                    }
                    
                    await _operationService.SetTransactionOperationCompleted(operation.Id);
                    await _daoDispatcher.VestingDeployed(data.UserId);
                }
                catch (Exception e)
                {
                    await _operationService.SetTransactionOperationFailed(operation.Id, e.Message);
                    await _daoDispatcher.VestingDeploymentFailed(data.UserId, e.Message);
                    _logger?.Error(e, "Vesting Deployment");
                    throw;
                }
            }
        }
    }
}