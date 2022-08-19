using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Project.GetProjectFee
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetProjectFeeQuery : IRequest<decimal>
    {
        public Guid ProjectId { get; set; }
    }
}