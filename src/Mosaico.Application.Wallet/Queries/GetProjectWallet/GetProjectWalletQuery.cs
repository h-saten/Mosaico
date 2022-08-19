using System;
using MediatR;
using Mosaico.Authorization.Base;
using Mosaico.Cache.Base.Attributes;

namespace Mosaico.Application.Wallet.Queries.GetProjectWallet
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    [Cache("{{ProjectId}}_{{Skip}}_{{Take}}", ExpirationInMinutes = 3)]
    public class GetProjectWalletQuery : IRequest<GetProjectWalletQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}