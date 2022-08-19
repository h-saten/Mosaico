using System.Collections.Generic;
using MediatR;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.Staking.GetStakingPairs
{
    public class GetStakingPairsQuery : IRequest<List<StakingPairDTO>>
    {
        
    }
}