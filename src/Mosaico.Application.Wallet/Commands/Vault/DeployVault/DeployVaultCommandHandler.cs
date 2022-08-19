using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Domain.Wallet.Entities.Enums;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Commands.Vault.DeployVault
{
    public class DeployVaultCommandHandler : IRequestHandler<DeployVaultCommand>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICurrentUserContext _userContext;
        private readonly IOperationService _operationService;
        
        public DeployVaultCommandHandler(IWalletDbContext walletDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher, ICurrentUserContext userContext, IOperationService operationService)
        {
            _walletDbContext = walletDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _userContext = userContext;
            _operationService = operationService;
        }

        public async Task<Unit> Handle(DeployVaultCommand request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null) throw new TokenNotFoundException(request.TokenId);
            if (token.VaultId.HasValue)
            {
                throw new VaultAlreadyExistsException(request.TokenId);
            }
            
            var operation = await _walletDbContext.Operations.OrderByDescending(t => t.CreatedAt)
                .FirstOrDefaultAsync(t => t.Network == token.Network && t.Type == BlockchainOperationType.VAULT_DEPLOYMENT && 
                                          t.TransactionId == token.Id, cancellationToken: cancellationToken);
            
            if (operation != null && (operation.State == OperationState.IN_PROGRESS ||
                                      operation.State == OperationState.SUCCESSFUL))
            {
                throw new VaultDeployingException(token.Id);
            }
            
            operation ??= await _operationService.CreateVaultDeploymentOperationAsync(token.Network, token.Id, _userContext.UserId);

            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets,
                new DeployVaultRequested(request.TokenId, _userContext.UserId, operation.Id));
            await _eventPublisher.PublishAsync(e);
            return Unit.Value;
        }
    }
}