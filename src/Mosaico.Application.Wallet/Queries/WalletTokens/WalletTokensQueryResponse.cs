using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.Queries.WalletTokens
{
    public class WalletTokensQueryResponse
    {
        public decimal Delta { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WalletDeltaDirection DeltaDirection { get; set; } = WalletDeltaDirection.NONE;
        public string Currency { get; set; }
        public string Address { get; set; }
        public decimal TotalWalletValue { get; set; }
        public List<TokenBalanceDTO> Tokens { get; set; }
    }
}