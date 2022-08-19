using System.Collections.Generic;
using MediatR;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.GetExternalExchanges
{
    public class GetExternalExchangesQuery : IRequest<List<ExternalExchangeDTO>>
    {
        
    }
}