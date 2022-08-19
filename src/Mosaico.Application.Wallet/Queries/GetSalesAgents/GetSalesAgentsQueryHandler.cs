using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetSalesAgents
{
    public class GetSalesAgentsQueryHandler : IRequestHandler<GetSalesAgentsQuery, List<SalesAgentDTO>>
    {
        private readonly IWalletDbContext _context;
        private readonly IMapper _mapper;

        public GetSalesAgentsQueryHandler(IWalletDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SalesAgentDTO>> Handle(GetSalesAgentsQuery request, CancellationToken cancellationToken)
        {
            var salesAgentQuery = _context.SalesAgents.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(request.Company))
                salesAgentQuery = salesAgentQuery.Where(a => a.Company == request.Company);
            var agents = await salesAgentQuery.ToListAsync(cancellationToken);
            return agents.Select(a => _mapper.Map<SalesAgentDTO>(a)).ToList();
        }
    }
}