using System;
using System.Collections.Generic;
using Mosaico.Application.Wallet.DTO;

namespace Mosaico.Application.Wallet.Queries.VentureFund.GetVentureFundHistory
{
    public class GetVentureFundHistoryQueryResponse
    {
        public List<TokenPriceHistoryDTO> Tokens { get; set; }
        public decimal TotalAssetValue { get; set; }
        public DateTimeOffset? LastUpdatedAt { get; set; }
    }
}