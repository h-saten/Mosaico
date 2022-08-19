using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Authorization.Base.Exceptions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Vesting.RedeployVesting
{
    public class RedeployVestingCommandHandler : IRequestHandler<RedeployVestingCommand>
    {
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWalletDbContext _walletDbContext;
        private readonly ICurrentUserContext _userContext;
        private readonly IUserManagementClient _managementClient;

        public RedeployVestingCommandHandler(IEventFactory eventFactory, IEventPublisher eventPublisher, ICurrentUserContext userContext, IWalletDbContext walletDbContext, IUserManagementClient managementClient)
        {
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _userContext = userContext;
            _walletDbContext = walletDbContext;
            _managementClient = managementClient;
        }

        public async Task<Unit> Handle(RedeployVestingCommand request, CancellationToken cancellationToken)
        {
            var vesting = await _walletDbContext.Vestings.FirstOrDefaultAsync(v => v.Id == request.VestingId, cancellationToken: cancellationToken);
            if (vesting == null) throw new VestingNotFoundException(request.VestingId);
            
            var userTokenPermissions = await _managementClient.GetUserPermissionsAsync(_userContext.UserId, vesting.TokenId, cancellationToken);
            if (!userTokenPermissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Token.CanEdit))
                throw new UnauthorizedException($"User is unauthorized for token {vesting.TokenId}");
            
            var operation = await _walletDbContext.Operations.FirstOrDefaultAsync(o =>
                o.Network == vesting.Token.Network && o.Type == BlockchainOperationType.VESTING_CREATION &&
                o.TransactionId == vesting.Id, cancellationToken: cancellationToken);
                
            if (operation != null && operation.State == OperationState.IN_PROGRESS)
            {
                throw new VestingCreationIsInProgressException(vesting.VaultId ?? Guid.Empty);
            }
            
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, 
                new VestingDeploymentRequested(request.VestingId, _userContext.UserId));
            await _eventPublisher.PublishAsync(e);
            return Unit.Value;
        }
    }
}