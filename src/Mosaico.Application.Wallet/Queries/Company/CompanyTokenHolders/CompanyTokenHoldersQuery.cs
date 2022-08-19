using System;
using MediatR;
using Mosaico.Authorization.Base;
using Newtonsoft.Json;

namespace Mosaico.Application.Wallet.Queries.Company.CompanyTokenHolders
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanReadCompanyWallet)]
    public class CompanyTokenHoldersQuery : IRequest<CompanyTokenHoldersQueryResponse>
    {
        [JsonIgnore]
        public Guid CompanyId { get; set; }
        public Guid? TokenId { get; set; }
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 30;
    }
}