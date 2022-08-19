using System.Collections.Generic;
using MediatR;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.GetSalesAgents
{
    public class GetSalesAgentsQuery : IRequest<List<SalesAgentDTO>>
    {
        public string Company { get; set; }
    }
}