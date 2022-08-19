using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertPaymentCurrency
{
    public class UpsertPaymentCurrencyCommandHandler : IRequestHandler<UpsertPaymentCurrencyCommand>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IWalletClient _walletClient;
        private readonly ICacheClient _cacheClient;
        private readonly IBusinessManagementClient _businessManagementClient;

        public UpsertPaymentCurrencyCommandHandler(
            ILogger logger, 
            IProjectDbContext projectDbContext, 
            IWalletClient walletClient, 
            ICacheClient cacheClient, 
            IBusinessManagementClient businessManagementClient)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _walletClient = walletClient;
            _cacheClient = cacheClient;
            _businessManagementClient = businessManagementClient;
        }

        public async Task<Unit> Handle(UpsertPaymentCurrencyCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create project document");
            
            var project = await _projectDbContext
                .Projects
                .Include(x => x.Crowdsale)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            if (project.CompanyId == null)
            {
                throw new ProjectCompanyNotFoundException(request.ProjectId);
            }

            var projectCompany = await _businessManagementClient.GetCompanyAsync(project.CompanyId.Value, cancellationToken);

            if (projectCompany is null)
            {
                throw new CompanyNotExistsException(project.CompanyId.Value.ToString());
            }
            
            _logger?.Verbose($"Project {request.ProjectId} was found");
            
            var availablePaymentCurrencies = await _walletClient.GetPaymentCurrencyAddressesAsync(projectCompany.Network);
            var currencyIsAvailable = availablePaymentCurrencies.Contains(request.PaymentCurrencyAddress);

            if (currencyIsAvailable is false)
            {
                throw new UnsupportedPaymentCurrencyException(request.PaymentCurrencyAddress);
            }
            
            project.Crowdsale ??= new Domain.ProjectManagement.Entities.Crowdsale();
            var paymentCurrencies = project.Crowdsale.SupportedStableCoins;
            var paymentCurrency = paymentCurrencies?.FirstOrDefault(x=> x == request.PaymentCurrencyAddress);
            
            // TODO if finish silently or throw exception?
            if (request.IsEnabled)
            {
                if (paymentCurrency is null)
                {
                    project.Crowdsale.SupportedStableCoins ??= new List<string>();
                    project.Crowdsale.SupportedStableCoins.Add(request.PaymentCurrencyAddress);
                };
            }
            else
            {
                if (paymentCurrency is not null)
                {
                    project.Crowdsale.SupportedStableCoins?.Remove(paymentCurrency);
                }
            }
            _projectDbContext.Crowdsales.Update(project.Crowdsale);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            await _cacheClient.CleanAsync(new List<string>
            {
                $"{nameof(GetProjectQuery)}_{project.Id}",
                $"{nameof(GetProjectQuery)}_{project.Slug}"
            }, cancellationToken);
            
            return Unit.Value;
        }
    }
}