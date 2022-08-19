using System;
using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.Vesting.GetWalletVestings
{
    public class GetWalletVestingsQueryResponse
    {
        public List<VestingWalletDTO> Items { get; set; }
    }
}