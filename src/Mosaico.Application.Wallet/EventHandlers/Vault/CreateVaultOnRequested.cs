using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Queries.GetToken;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Identity;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.EventHandlers.Vault
{
    [EventInfo(nameof(CreateVaultOnRequested),  "wallets:api")]
    [EventTypeFilter(typeof(DeployVaultRequested))]
    public class CreateVaultOnRequested : EventHandlerBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IVaultv1Service _vaultv1Service;
        private readonly IDaoDispatcher _daoDispatcher;
        private readonly ILogger _logger;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly IOperationService _operationService;
        private readonly ICacheClient _cacheClient;

        public CreateVaultOnRequested(IWalletDbContext walletDbContext, ILogger logger, IVaultv1Service vaultv1Service, ICompanyWalletService companyWalletService, IDaoDispatcher daoDispatcher, ICacheClient cacheClient, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _vaultv1Service = vaultv1Service;
            _companyWalletService = companyWalletService;
            _daoDispatcher = daoDispatcher;
            _cacheClient = cacheClient;
            _operationService = operationService;
            _logger = logger.ForContext(typeof(CreateVaultOnRequested));
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event.GetData<DeployVaultRequested>();
            if (data != null)
            {
                var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == data.TokenId);
                if (token == null)
                {
                    _logger.Warning($"Token {data.TokenId} not found");
                    return;
                }

                if (token.VaultId.HasValue)
                {
                    _logger.Warning($"Token {token.Symbol} already has vault");
                    return;
                }
                
                var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o => o.Id == data.OperationId);
                if (operation != null && (operation.State == OperationState.IN_PROGRESS ||
                                          operation.State == OperationState.SUCCESSFUL))
                {
                    return;
                }
                
                operation ??= await _operationService.CreateVaultDeploymentOperationAsync(token.Network, token.Id, data.UserId);

                try
                {
                    await _operationService.SetTransactionInProgress(operation.Id, null);
                    var account = await _companyWalletService.GetAccountAsync(token.CompanyId, token.Network);
                    var contractAddress = await _vaultv1Service.DeployAsync(account, token.Network);
                    var vault = new Domain.Wallet.Entities.Vault
                    {
                        Address = contractAddress,
                        Network = token.Network,
                        Token = token,
                        TokenId = token.Id,
                        CompanyId = token.CompanyId
                    };
                    _walletDbContext.Vaults.Add(vault);
                    await _walletDbContext.SaveChangesAsync();
                    await _operationService.SetTransactionOperationCompleted(operation.Id, hash: contractAddress);
                    await _cacheClient.CleanAsync($"{nameof(GetTokenQuery)}_{token.Id}");
                    await _daoDispatcher.VaultDeployed(data.UserId);
                }
                catch (Exception ex)
                {
                    await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                    _logger.Error(ex, "Vault deployment failure");
                    await _daoDispatcher.VaultDeploymentFailed(data.UserId, ex.Message);
                }
            }
        }
    }
}