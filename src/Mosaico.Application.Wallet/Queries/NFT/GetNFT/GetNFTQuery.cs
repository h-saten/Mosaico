using System;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Base;

namespace Mosaico.Application.Wallet.Queries.NFT.GetNFT
{
    public class GetNFTQuery : IRequest<PaginatedResult<NFTDTO>>
    {
        public Guid? ProjectId { get; set; }
        public Guid? CompanyId { get; set; }
        public Guid? NFTCollectionId { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}