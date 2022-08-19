using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;

namespace Mosaico.Application.Wallet.Commands.NFT.CreateNFT
{
    public class CreateNFTCommandHandler : IRequestHandler<CreateNFTCommand, Guid>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        
        public CreateNFTCommandHandler(IWalletDbContext walletDbContext, IEventFactory eventFactory, IEventPublisher eventPublisher)
        {
            _walletDbContext = walletDbContext;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
        }

        public async Task<Guid> Handle(CreateNFTCommand request, CancellationToken cancellationToken)
        {
            var collection =
                await _walletDbContext.NFTCollections.FirstOrDefaultAsync(c => c.Id == request.NFTCollectionId, cancellationToken);
            if (collection == null)
            {
                throw new NFTCollectionNotFoundException(request.NFTCollectionId);
            }
        
            if (collection.NFTs.Any(n => n.TokenId == request.TokenId))
            {
                throw new NFTAlreadyExistsException(request.TokenId);
            }
            var nft = new Domain.Wallet.Entities.NFT
            {
                Uri = request.Uri,
                OwnerAddress = request.OwnerAddress,
                TokenId = request.TokenId,
                NFTCollection = collection,
                NFTCollectionId = collection.Id
            };
            collection.NFTs.Add(nft);
            await _walletDbContext.SaveChangesAsync(cancellationToken);
            await PublishEventAsync(nft);
            return nft.Id;
        }
        
        private async Task PublishEventAsync(Domain.Wallet.Entities.NFT nft)
        {
            var e = _eventFactory.CreateEvent(Events.Wallet.Constants.EventPaths.Wallets, new NFTCreated(nft.NFTCollectionId, nft.Id, nft.TokenId));
            await _eventPublisher.PublishAsync(e);
        }
    }
}