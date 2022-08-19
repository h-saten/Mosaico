using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.GetActiveBlockchains
{
    public class GetActiveBlockchainsQueryResponse
    {
        public List<BlockchainConfigurationDTO> Networks { get; set; }
    }
}