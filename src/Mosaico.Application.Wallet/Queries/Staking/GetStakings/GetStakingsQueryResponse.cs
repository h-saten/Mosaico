using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.Staking.GetStakings
{
    public class GetStakingsQueryResponse
    {
        public List<StakingDTO> Stakings { get; set; }
    }
}