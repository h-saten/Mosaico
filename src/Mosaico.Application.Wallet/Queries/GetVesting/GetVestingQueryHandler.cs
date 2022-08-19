using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Queries.GetVesting
{
    public class GetVestingQueryHandler : IRequestHandler<GetVestingQuery, GetVestingQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IWalletDbContext _projectDbContext;
        private readonly IMapper _mapper;
        
        public GetVestingQueryHandler(IWalletDbContext projectDbContext, IMapper mapper, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetVestingQueryResponse> Handle(GetVestingQuery request, CancellationToken cancellationToken)
        {
            var vesting = await _projectDbContext.Vestings.FirstOrDefaultAsync(v => v.TokenId == request.TokenId, cancellationToken);
            if (vesting == null)
            {
                throw new VestingNotFoundException(request.TokenId);
            }

            return new GetVestingQueryResponse
            {
                Vesting = _mapper.Map<VestingDTO>(vesting)
            };
        }
    }
}