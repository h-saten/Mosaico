using System;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class DeflationDTO
    {
        public Guid Id { get; set; }
        public decimal? TransactionPercentage { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public DeflationType Type { get; set; }
        public long? BuyoutDelayInDays { get; set; }
        public decimal? BuyoutPercentage { get; set; }
        public DateTimeOffset? StartsAt { get; set; }
    }
}