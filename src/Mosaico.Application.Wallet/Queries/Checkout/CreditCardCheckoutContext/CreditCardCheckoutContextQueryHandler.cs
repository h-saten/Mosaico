using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Checkout.CreditCardCheckoutContext
{
    public class CreditCardCheckoutContextQueryHandler : IRequestHandler<CreditCardCheckoutContextQuery, CreditCardCheckoutContextQueryResponse>
    {
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IProjectWalletService _projectWalletService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IBusinessManagementClient _businessManagement;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _userContext;

        public CreditCardCheckoutContextQueryHandler(IExchangeRateService exchangeRateService, IProjectWalletService projectWalletService, IProjectManagementClient projectManagementClient, IBusinessManagementClient businessManagement, IWalletDbContext walletDbContext, ICurrentUserContext userContext)
        {
            _exchangeRateService = exchangeRateService;
            _projectWalletService = projectWalletService;
            _projectManagementClient = projectManagementClient;
            _businessManagement = businessManagement;
            _walletDbContext = walletDbContext;
            _userContext = userContext;
        }

        public async Task<CreditCardCheckoutContextQueryResponse> Handle(CreditCardCheckoutContextQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var company = await _businessManagement.GetCompanyAsync(project.CompanyId.Value, cancellationToken);
            if (company == null)
            {
                throw new CompanyNotExistsException(project.CompanyId.ToString());
            }

            var projectWallet =
                await _walletDbContext.ProjectWallets.FirstOrDefaultAsync(w =>
                    w.ProjectId == project.Id && w.Network == company.Network, cancellationToken: cancellationToken);
            
            if(projectWallet == null)
            {
                projectWallet = await _projectWalletService.CreateWalletAsync(company.Network, project.Id);
            }

            var account =
                await _projectWalletService.GetAccountAsync(company.Network, project.Id, request.UserId);

            var exchangeRates = await _exchangeRateService.GetExchangeRatesAsync();
            var regulation = await _projectManagementClient.GetProjectDocumentAsync(project.Id, "TERMS_AND_CONDITIONS", _userContext.Language, cancellationToken);
            var policy = await _projectManagementClient.GetProjectDocumentAsync(project.Id, "PRIVACY_POLICY", _userContext.Language, cancellationToken);
            var stage = await _projectManagementClient.CurrentProjectSaleStage(project.Id, cancellationToken);
            var limit = stage.PurchaseLimits.FirstOrDefault(pl => pl.PaymentMethod == Domain.ProjectManagement.Constants.PaymentMethods.CreditCard);
            
            return new CreditCardCheckoutContextQueryResponse
            {
                ExchangeRates = exchangeRates.Select(er => new ExchangeRateDTO
                {
                    Currency = er.Ticker,
                    BaseCurrency = er.BaseCurrency,
                    ExchangeRate = er.Rate
                }).ToList(),
                PaymentAddress = account.Address,
                ProjectName = project.Title,
                CompanyName = company.Name,
                RegulationsUrl = regulation?.Url,
                PrivacyPolicyUrl = policy?.Url,
                MaximumPurchase = limit?.MaximumPurchase ?? stage.MaximumPurchase,
                MinimumPurchase = limit?.MinimumPurchase ?? stage.MinimumPurchase
            };
        }
    }
}