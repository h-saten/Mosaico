using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Queries.Company.GetCompanyOwnedTokens
{
    public class GetCompanyOwnedTokensQueryHandler : IRequestHandler<GetCompanyOwnedTokensQuery, List<TokenDTO>>
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetCompanyOwnedTokensQueryHandler(IWalletDbContext dbContext, IMapper mapper, ILogger logger = null)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<List<TokenDTO>> Handle(GetCompanyOwnedTokensQuery request, CancellationToken cancellationToken)
        {
            var tokens = await _dbContext.Tokens
                .Where(t => t.CompanyId == request.CompanyId)
                .ToListAsync(cancellationToken);
            return tokens.Select(t => _mapper.Map<TokenDTO>(t)).ToList();
        }
    }
}