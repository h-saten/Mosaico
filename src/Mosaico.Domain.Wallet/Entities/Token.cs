using System;
using System.Collections.Generic;
using Mosaico.Domain.Base;
using Mosaico.Domain.Wallet.Entities.Enums;

namespace Mosaico.Domain.Wallet.Entities
{
    public class Token : EntityBase
    {
        public string Name { get; set; }
        public string NameNormalized { get; set; }
        public string Symbol { get; set; }
        public string SymbolNormalized { get; set; }
        public string Address { get; set; }
        public string OwnerAddress { get; set; }
        public decimal TotalSupply { get; set; }
        public decimal TokensLeft { get; set; }
        public string Network { get; set; }
        public TokenStatus Status { get; set; }
        public bool IsBurnable { get; set; }
        public bool IsMintable { get; set; }
        public virtual List<Staking.Staking> Stakings { get; set; } = new List<Staking.Staking>();
        public virtual List<Vesting> Vestings { get; set; } = new List<Vesting>();
        public Guid TypeId { get; set; }
        public string LegacyId { get; set; }
        public bool IsGovernance { get; set; }
        public int Decimals { get; set; }
        public virtual TokenType Type { get; set; }
        public virtual List<WalletToToken> Wallets { get; set; } = new List<WalletToToken>();
        public virtual List<CompanyWalletToToken> CompanyWallets { get; set; } = new List<CompanyWalletToToken>();
        public string ContractVersion { get; set; }
        public string LogoUrl { get; set; }
        public Guid CompanyId { get; set; }
        public virtual List<TokenToExternalExchange> Exchanges { get; set; } = new List<TokenToExternalExchange>();
        public virtual List<TokenDistribution> Distributions { get; set; } = new List<TokenDistribution>();
        public virtual Vault Vault { get; set; }
        public Guid? VaultId { get; set; }
        public Guid? DeflationId { get; set; }
        public virtual Deflation Deflation { get; set; }
        public bool DisplayAlways { get; set; }
        public bool IsVestingEnabled { get; set; }
        public bool IsStakingEnabled { get; set; }
        public DateTimeOffset? StakingStartsAt { get; set; }
        public bool IsDeflationary { get; set; }
        
        public ERCType ERCType { get; set; }

        public string GetERCType()
        {
            return ERCType.ToString();
        }
        
        public void SetType(TokenType type)
        {
            Type = type;
            TypeId = type.Id;
        }
    }
}