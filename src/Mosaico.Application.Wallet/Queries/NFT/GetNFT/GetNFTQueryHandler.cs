using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Base;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.NFT.GetNFT
{
    public class GetNFTQueryHandler : IRequestHandler<GetNFTQuery, PaginatedResult<NFTDTO>>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public GetNFTQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<PaginatedResult<NFTDTO>> Handle(GetNFTQuery request, CancellationToken cancellationToken)
        {
            var query = _walletDbContext.NFTs.AsQueryable();
            if (request.NFTCollectionId.HasValue)
            {
                query = query.Where(n => n.NFTCollectionId == request.NFTCollectionId.Value);
            }

            if (request.ProjectId.HasValue)
            {
                query = query.Where(n => n.NFTCollection.ProjectId == request.ProjectId);
            }

            if (request.CompanyId.HasValue)
            {
                query = query.Where(n => n.NFTCollection.CompanyId == request.CompanyId);
            }
            
            var nfts = await query.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
            var totalCount = await query.CountAsync(cancellationToken);
            return new PaginatedResult<NFTDTO>
            {
                Entities = nfts.Select(n => _mapper.Map<NFTDTO>(n)),
                Total = totalCount
            };
        }
    }
}