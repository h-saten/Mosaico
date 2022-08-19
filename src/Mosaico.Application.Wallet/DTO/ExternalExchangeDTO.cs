using System;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class ExternalExchangeDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LogoUrl { get; set; }
        public bool IsDisabled { get; set; }
        public string Url { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ExternalExchangeType Type { get; set; }
    }
}