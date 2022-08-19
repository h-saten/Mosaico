using System;
using System.Collections.Generic;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class VestingFundDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal TokenAmount { get; set; }
        public int Days { get; set; }
        public DateTimeOffset? StartAt { get; set; }
        public Guid VestingId { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public VestingFundStatus Status { get; set; }
        public string FailureReason { get; set; }
    }
}