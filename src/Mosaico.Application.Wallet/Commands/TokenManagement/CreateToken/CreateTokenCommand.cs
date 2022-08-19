using System;
using MediatR;
using Mosaico.Application.Wallet.Queries.Company.GetCompanyOwnedTokens;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.CreateToken
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    [CacheReset(nameof(GetCompanyOwnedTokensQuery), "{{CompanyId}}")]
    public class CreateTokenCommand : IRequest<Guid>
    {
        public string Name{ get; set; }
        public string Symbol { get; set; }
        public int Decimals { get; set; }
        public string Network { get; set; }
        public decimal InitialSupply { get; set; }
        public string TokenType { get; set; }
        public bool IsMintable { get; set; }
        public bool IsBurnable { get; set; }
        public Guid CompanyId { get; set; }
        public Guid? ProjectId { get; set; }
        public string OwnerAddress { get; set; }
        public string ContractAddress { get; set; }
        public string ContractVersion { get; set; }
        public bool IsGovernance { get; set; }
    }
}