using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Vesting.GetPersonalVestings
{
    public class GetPersonalVestingsQueryHandler : IRequestHandler<GetPersonalVestingsQuery, GetPersonalVestingsQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;

        public GetPersonalVestingsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
        }

        public async Task<GetPersonalVestingsQueryResponse> Handle(GetPersonalVestingsQuery request, CancellationToken cancellationToken)
        {
            var vestings = await _walletDbContext.Vestings.Where(v => v.TokenId == request.TokenId)
                .Include(v => v.Funds).AsNoTracking().ToListAsync(cancellationToken);
            return new GetPersonalVestingsQueryResponse
            {
                Vestings = vestings.Select(v => _mapper.Map<VestingDTO>(v)).ToList()
            };
        }
    }
}