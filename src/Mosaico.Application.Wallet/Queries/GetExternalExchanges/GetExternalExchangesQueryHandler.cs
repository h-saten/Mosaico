using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetExternalExchanges
{
    public class GetExternalExchangesQueryHandler : IRequestHandler<GetExternalExchangesQuery, List<ExternalExchangeDTO>>
    {
        private readonly IWalletDbContext _context;
        private readonly IMapper _mapper;
        
        public GetExternalExchangesQueryHandler(IWalletDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public Task<List<ExternalExchangeDTO>> Handle(GetExternalExchangesQuery request, CancellationToken cancellationToken)
        {
            return _context
                .ExternalExchanges
                .AsNoTracking()
                .Select(t => _mapper.Map<ExternalExchangeDTO>(t))
                .ToListAsync(cancellationToken);
        }
    }
}