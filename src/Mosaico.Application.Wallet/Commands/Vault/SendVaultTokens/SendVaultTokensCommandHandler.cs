using System;
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
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Commands.Vault.SendVaultTokens
{
    public class SendVaultTokensCommandHandler : IRequestHandler<SendVaultTokensCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IUserManagementClient _managementClient;
        
        public SendVaultTokensCommandHandler(IWalletDbContext walletDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ICurrentUserContext currentUserContext, IUserManagementClient managementClient)
        {
            _walletDbContext = walletDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _currentUserContext = currentUserContext;
            _managementClient = managementClient;
        }

        public async Task<Unit> Handle(SendVaultTokensCommand request, CancellationToken cancellationToken)
        {
            var vault = await _walletDbContext.Vaults.FirstOrDefaultAsync(v => v.Id == request.VaultId, cancellationToken);
            if (vault == null)
                throw new VaultNotFoundException(Guid.Empty);
            
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == vault.TokenId, cancellationToken);
            if (token == null) throw new TokenNotFoundException(vault.TokenId);
            
            var userPermissions = await _managementClient.GetUserPermissionsAsync(_currentUserContext.UserId, token.Id, cancellationToken);
            if (!userPermissions.Any(p => p.Key == Authorization.Base.Constants.Permissions.Token.CanEdit))
                throw new UnauthorizedException($"User has not permissions over token {token.Id}");
            
            var distribution = token.Distributions.FirstOrDefault(d => d.Id == request.TokenDistributionId);
            if (distribution == null) throw new TokenDistributionNotFoundException(request.TokenDistributionId.ToString());
            
            var operation = await _walletDbContext.Operations.OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync(t =>
                    t.Network == vault.Token.Network && t.TransactionId == vault.Id && t.Type == BlockchainOperationType.VAULT_TRANSFER, cancellationToken: cancellationToken);
            
            if (operation != null && operation.State == OperationState.IN_PROGRESS)
            {
                throw new VaultTransferInProgressException(vault.Id);
            }
            
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new VaultSendRequested(distribution.Id, request.Amount, request.Recipient, _currentUserContext.UserId));
            await _eventPublisher.PublishAsync(e);
            return Unit.Value;
        }
    }
}