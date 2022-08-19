using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Mosaico.Domain.Wallet.Entities.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mosaico.Application.Wallet.DTO
{
    public class TokenProjectDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string LogoUrl { get; set; }
    }
    
    public class TokenDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Address { get; set; }
        public string Network { get; set; }
        public bool IsBurnable { get; set; }
        public bool IsMintable { get; set; }
        public string LogoUrl { get; set; }
        public List<TokenExternalExchangeDTO> Exchanges { get; set; } = new List<TokenExternalExchangeDTO>();
        public decimal TotalSupply { get; set; }
        public string Type { get; set; }
        
        [JsonConverter(typeof(StringEnumConverter))]
        public TokenStatus Status { get; set; }
        
        public Guid CompanyId { get; set; }
        public bool IsGovernance { get; set; }
        public List<TokenProjectDTO> Projects { get; set; } = new List<TokenProjectDTO>();
        public List<StakingDTO> Stakings { get; set; }
        public VestingDTO Vesting { get; set; }
        public DeflationDTO Deflation { get; set; }
        public VaultDTO Vault { get; set; }
        public bool IsVestingEnabled { get; set; }
        public bool IsStakingEnabled { get; set; }
        public DateTimeOffset? StakingStartsAt { get; set; }
        public bool IsDeflationary { get; set; }
    }
}