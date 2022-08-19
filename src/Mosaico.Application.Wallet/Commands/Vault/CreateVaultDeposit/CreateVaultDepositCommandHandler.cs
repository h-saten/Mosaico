using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Authorization.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Vault.CreateVaultDeposit
{
    public class CreateVaultDepositCommandHandler : IRequestHandler<CreateVaultDepositCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserContext _userContext;
        private readonly IUserManagementClient _managementClient;

        public CreateVaultDepositCommandHandler(IWalletDbContext walletDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ICurrentUserContext userContext, IUserManagementClient managementClient)
        {
            _walletDbContext = walletDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _userContext = userContext;
            _managementClient = managementClient;
        }

        public async Task<Unit> Handle(CreateVaultDepositCommand request, CancellationToken cancellationToken)
        {
            var tokenDistribution = await _walletDbContext.TokenDistributions.FirstOrDefaultAsync(t =>
                t.Id == request.TokenDistributionId, cancellationToken: cancellationToken);
            if(tokenDistribution == null) throw new TokenDistributionNotFoundException(request.TokenDistributionId.ToString());

            var permissions = await _managementClient.GetUserPermissionsAsync(_userContext.UserId,
                tokenDistribution.TokenId, cancellationToken);
            
            if (!permissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Token.CanEdit) && !_userContext.IsGlobalAdmin)
                throw new UnauthorizedException($"User has not permissions over token {tokenDistribution.TokenId}");
            
            if (!string.IsNullOrWhiteSpace(tokenDistribution.SmartContractId))
            {
                throw new TokenDistributionAlreadyDeployedException(request.TokenDistributionId);
            }

            var operation = await _walletDbContext.Operations.OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync(t =>
                    t.Network == tokenDistribution.Token.Network && t.TransactionId == tokenDistribution.Id && t.Type == BlockchainOperationType.DEPOSIT_DEPLOYMENT, cancellationToken: cancellationToken);
            
            if (operation != null && operation.State == OperationState.IN_PROGRESS)
            {
                throw new DepositIsDeployingException(tokenDistribution.Id);
            }

            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new CreateVaultDepositRequested(tokenDistribution.Id, _userContext.UserId));
            await _eventPublisher.PublishAsync(e);
            return Unit.Value;
        }
    }
}