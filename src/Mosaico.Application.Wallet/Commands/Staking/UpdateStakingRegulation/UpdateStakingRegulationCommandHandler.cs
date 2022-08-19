using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities.Staking;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;
using Serilog;

namespace Mosaico.Application.Wallet.Commands.Staking.UpdateStakingRegulation
{
    public class UpdateStakingRegulationCommandHandler : IRequestHandler<UpdateStakingRegulationCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserManagementClient _userManagementClient;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ILogger _logger;

        public UpdateStakingRegulationCommandHandler(IWalletDbContext walletDbContext, ILogger logger, IUserManagementClient userManagementClient, ICurrentUserContext currentUserContext)
        {
            _walletDbContext = walletDbContext;
            _logger = logger;
            _userManagementClient = userManagementClient;
            _currentUserContext = currentUserContext;
        }

        public async Task<Unit> Handle(UpdateStakingRegulationCommand request, CancellationToken cancellationToken)
        {
            var stakingPair = await _walletDbContext.StakingPairs.FirstOrDefaultAsync(t => t.Id == request.StakingPairId, cancellationToken: cancellationToken);
            if (stakingPair == null)
            {
                throw new StakingPairNotFoundException(request.StakingPairId);
            }

            var tokenId = stakingPair.StakingTokenId;
            if (!tokenId.HasValue) throw new InvalidOperationException("Staking is not assigned to token");
            
            var userPermissions = await _userManagementClient.GetUserPermissionsAsync(_currentUserContext.UserId, tokenId.Value, cancellationToken);
            if (!userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Token.CanEdit, tokenId.Value))
            {
                throw new ForbiddenException();
            }

            stakingPair.StakingRegulation ??= new StakingRegulation();
            stakingPair.StakingRegulation.UpdateTranslation(request.Regulation, request.Language);
            _walletDbContext.StakingPairs.Update(stakingPair);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}