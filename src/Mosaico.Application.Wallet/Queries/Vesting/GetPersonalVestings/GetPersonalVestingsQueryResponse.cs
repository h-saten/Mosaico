using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.Vesting.GetPersonalVestings
{
    public class GetPersonalVestingsQueryResponse
    {
        public List<VestingDTO> Vestings { get; set; }
    }
}