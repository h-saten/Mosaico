using System;
using System.Collections.Generic;

namespace Mosaico.Application.KangaWallet.Queries.TokenEstimates
{
    [Serializable]
    public class TokenEstimatesResponseDto
    {
        public List<CurrencyEstimateDto> Estimates { get; set; }
    }
}