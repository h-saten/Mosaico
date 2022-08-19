using System;
using MediatR;

namespace Mosaico.Application.KangaWallet.Queries.TokenEstimates
{
    public class TokenEstimatesQuery : IRequest<TokenEstimatesResponseDto>
    {
        public string UserId { get; set; }
        public Guid TokenId { get; set; }
    }
}
