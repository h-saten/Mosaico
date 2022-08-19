using System;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetToken
{
    // [Cache("{{Id}}")]
    public class GetTokenQuery : IRequest<TokenDTO>
    {
        public Guid Id { get; set; }
    }
}