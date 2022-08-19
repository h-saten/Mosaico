using MediatR;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetActiveBlockchains
{
    [Cache]
    public class GetActiveBlockchainsQuery : IRequest<GetActiveBlockchainsQueryResponse>
    {
        
    }
}