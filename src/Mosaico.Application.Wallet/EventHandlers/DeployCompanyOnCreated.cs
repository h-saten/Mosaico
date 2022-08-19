using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Events.Wallet;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.SignalR.Abstractions;
using Mosaico.Integration.SignalR.DTO;
using Mosaico.SDK.BusinessManagement.Abstractions;

namespace Mosaico.Application.Wallet.EventHandlers
{
    [EventInfo(nameof(DeployCompanyOnCreated),  "companies:api")]
    [EventTypeFilter(typeof(CompanyCreatedEvent))]
    public class DeployCompanyOnCreated : EventHandlerBase
    {
        private readonly IDaoService _daoService;
        private readonly IBusinessManagementClient _businessClient;
        private readonly IWalletDbContext _context;
        private readonly ICompanyWalletService _companyWalletService;
        private readonly IDaoDispatcher _companyCreatedDispatcher;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IOperationService _operationService;

        public DeployCompanyOnCreated(IDaoService daoService,  IBusinessManagementClient businessClient, IWalletDbContext context, IDaoDispatcher companyCreatedDispatcher, IEventPublisher eventPublisher, IEventFactory eventFactory, ICompanyWalletService companyWalletService, IOperationService operationService)
        {
            _daoService = daoService;
            _businessClient = businessClient;
            _context = context;
            _companyCreatedDispatcher = companyCreatedDispatcher;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _companyWalletService = companyWalletService;
            _operationService = operationService;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<CompanyCreatedEvent>();
            if (data != null)
            {
                var company = await _businessClient.GetCompanyAsync(data.Id);
                if (company != null)
                {
                    var companyWallet = await _companyWalletService.CreateCompanyWalletAsync(company.Id, company.Network);
                    var userWallet = await _context.Wallets.FirstOrDefaultAsync(t =>
                        t.Network == company.Network && t.UserId == data.CreatedById);
                    if (userWallet == null)
                    {
                        throw new WalletNotFoundException(data.CreatedById);
                    }

                    var operation = await _context.Operations.FirstOrDefaultAsync(t =>
                        t.Network == companyWallet.Network && t.Type == BlockchainOperationType.DAO_DEPLOYMENT
                                                           && t.TransactionId == company.Id);
                    if (operation != null && (operation.State == OperationState.IN_PROGRESS ||
                                              operation.State == OperationState.SUCCESSFUL))
                    {
                        throw new DaoDeploymentInProgressException(company.Id);
                    }
                    
                    operation ??= await _operationService.CreateDAODeploymentOperationAsync(companyWallet.Network, company.Id, data.CreatedById);
                    await _operationService.SetTransactionInProgress(operation.Id, null);
                    
                    try
                    {
                        var contract = await _daoService.DeployServiceAsync(company.Network, configuration =>
                        {
                            configuration.Name = company.Slug;
                            configuration.Owner = companyWallet.AccountAddress;
                            configuration.Quorum = company.Quorum;
                            configuration.OnlyOwnerProposals = company.OnlyOwnerProposals;
                            configuration.IsVotingEnabled = company.IsVotingEnabled;
                            configuration.PrivateKey = userWallet.PrivateKey;
                        });
                        await _businessClient.SetContractAddressAsync(company.Id, contract);
                        await _companyCreatedDispatcher.DispatchSucceededAsync(data.CreatedById, new CompanyCreatedDTO
                        {
                            Slug = company.Slug,
                            CompanyId = company.Id
                        });
                        await _operationService.SetTransactionOperationCompleted(operation.Id, hash: contract);
                        var e = _eventFactory.CreateEvent(Events.BusinessManagement.Constants.EventPaths.Companies, new DAOCreatedEvent(company.Id, company.Network));
                        await _eventPublisher.PublishAsync(e);
                    }
                    catch (Exception ex)
                    {
                        await _operationService.SetTransactionOperationFailed(operation.Id, ex.Message);
                        await _businessClient.DeleteCompanyAsync(company.Id);
                        await _companyCreatedDispatcher.DispatchFailedAsync(data.CreatedById, ex.Message);
                        throw;
                    }
                }
            }
        }
    }
}