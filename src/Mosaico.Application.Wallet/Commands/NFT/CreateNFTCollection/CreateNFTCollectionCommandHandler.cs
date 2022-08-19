using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Base.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Commands.NFT.CreateNFTCollection
{
    public class CreateNFTCollectionCommandHandler : IRequestHandler<CreateNFTCollectionCommand, Guid>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserManagementClient _managementClient;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;

        public CreateNFTCollectionCommandHandler(IWalletDbContext walletDbContext, IUserManagementClient managementClient, ICurrentUserContext currentDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher)
        {
            _walletDbContext = walletDbContext;
            _managementClient = managementClient;
            _currentUserContext = currentDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(CreateNFTCollectionCommand request, CancellationToken cancellationToken)
        {
            if (request.CompanyId.HasValue)
            {
                var companyPermissions = await _managementClient.GetUserPermissionsAsync(_currentUserContext.UserId, request.CompanyId.Value, cancellationToken);
                if (!companyPermissions.Any(c =>
                    c.Key == Authorization.Base.Constants.Permissions.Company.CanEditDetails) && !_currentUserContext.IsGlobalAdmin)
                {
                    throw new ForbiddenException();
                }
            }

            if (request.ProjectId.HasValue)
            {
                var projectPermissions = await _managementClient.GetUserPermissionsAsync(_currentUserContext.UserId, request.ProjectId.Value, cancellationToken);
                if (!projectPermissions.Any(c =>
                    c.Key == Authorization.Base.Constants.Permissions.Project.CanEditDetails) && !_currentUserContext.IsGlobalAdmin)
                {
                    throw new ForbiddenException();
                }
            }

            var collection = new NFTCollection
            {
                Name = request.Name,
                Network = request.Network,
                Symbol = request.Symbol,
                UserId = _currentUserContext.UserId,
                CompanyId = request.CompanyId,
                ProjectId = request.ProjectId,
                Address = request.Address
            };
            _walletDbContext.NFTCollections.Add(collection);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            await PublishEventAsync(collection);
            return collection.Id;
        }

        private async Task PublishEventAsync(NFTCollection collection)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, new NFTCollectionCreated(collection.UserId, collection.Id, collection.Address));
            await _eventPublisher.PublishAsync(e);
        }
    }
}