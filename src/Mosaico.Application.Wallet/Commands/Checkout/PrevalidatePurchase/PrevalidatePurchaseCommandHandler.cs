using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Checkout.PrevalidatePurchase
{
    public class PrevalidatePurchaseCommandHandler : IRequestHandler<PrevalidatePurchaseCommand, PrevalidatePurchaseCommandResponse>
    {
        private readonly ICrowdsalePurchaseService _crowdsalePurchaseService;
        private readonly IProjectManagementClient _projectManagementClient;
        private readonly IExchangeRateService _exchangeRateService;
        private readonly IUserManagementClient _managementClient;

        public PrevalidatePurchaseCommandHandler(ICrowdsalePurchaseService crowdsalePurchaseService, IProjectManagementClient projectManagementClient, IExchangeRateService exchangeRateService, IUserManagementClient managementClient)
        {
            _crowdsalePurchaseService = crowdsalePurchaseService;
            _projectManagementClient = projectManagementClient;
            _exchangeRateService = exchangeRateService;
            _managementClient = managementClient;
        }

        public async Task<PrevalidatePurchaseCommandResponse> Handle(PrevalidatePurchaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _projectManagementClient.GetProjectDetailsAsync(request.ProjectId, cancellationToken);
                    
                if (project == null)
                    throw new ProjectNotFoundException(request.ProjectId);

                if (project.SaleInProgress is false)
                    throw new InvalidProjectStatusException(request.ProjectId.ToString());
            
                var currentSaleStage = await _projectManagementClient.CurrentProjectSaleStage(project.Id, cancellationToken);
                if (currentSaleStage == null)
                {
                    throw new ProjectStageNotExistException(project.Id);
                }

                if (request.PaymentMethod != Domain.ProjectManagement.Constants.PaymentMethods.KangaExchange)
                {
                    var exchangeRate = await _exchangeRateService.GetExchangeRateAsync(request.Currency);
                    if (exchangeRate == null)
                    {
                        throw new InvalidExchangeRateException(request.Currency);
                    }

                    var estimatedTokenAmount =
                        ((request.PayedAmount * exchangeRate.Rate) / currentSaleStage.TokenPrice);
                    var tokenAmountDifference = request.TokenAmount - estimatedTokenAmount;
                    if (tokenAmountDifference > 0.1m)
                    {
                        request.TokenAmount = estimatedTokenAmount;
                    }
                }

                var canPurchase = await _crowdsalePurchaseService.CanPurchaseAsync(request.UserId, request.TokenAmount, currentSaleStage.Id, request.PaymentMethod);
                if (!canPurchase)
                {
                    throw new UnauthorizedPurchaseException(request.UserId, "Cannot purchase the project");
                }

                var user = await _managementClient.GetUserAsync(request.UserId, cancellationToken);

                return new PrevalidatePurchaseCommandResponse
                {
                    Status = "OK",
                    IsPhoneNumberRequired = string.IsNullOrWhiteSpace(user?.PhoneNumber)
                };
            }
            catch (ExceptionBase e)
            {
                return new PrevalidatePurchaseCommandResponse
                {
                    Status = e.Code,
                    Message = e.Message
                };
            }
        }
    }
}