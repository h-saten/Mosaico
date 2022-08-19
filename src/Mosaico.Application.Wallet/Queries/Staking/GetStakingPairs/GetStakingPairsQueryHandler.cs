using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Abstractions;

namespace Mosaico.Application.Wallet.Queries.Staking.GetStakingPairs
{
    public class GetStakingPairsQueryHandler : IRequestHandler<GetStakingPairsQuery, List<StakingPairDTO>>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;

        public GetStakingPairsQueryHandler(IWalletDbContext walletDbContext, IMapper mapper, ICurrentUserContext currentUserContext)
        {
            _walletDbContext = walletDbContext;
            _mapper = mapper;
            _currentUserContext = currentUserContext;
        }

        public async Task<List<StakingPairDTO>> Handle(GetStakingPairsQuery request, CancellationToken cancellationToken)
        {
            var stakingPairs = await _walletDbContext.StakingPairs.Where(t => t.IsEnabled).ToListAsync(cancellationToken);
            var dtos = new List<StakingPairDTO>();
            foreach (var stakingPair in stakingPairs)
            {
                var dto = _mapper.Map<StakingPairDTO>(stakingPair);
                dto.StakingRegulation = stakingPair.StakingRegulation?.GetTranslationInLanguage(_currentUserContext.Language)?.Value;
                dto.TermsAndConditionsUrl = stakingPair.StakingTerms?.GetTranslationInLanguage(_currentUserContext.Language)?.Value;
                dtos.Add(dto);
            }

            return dtos;
        }
    }
}