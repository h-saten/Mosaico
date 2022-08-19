using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Queries.Project.GetProjectInvestor
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class GetProjectInvestorQuery : IRequest<GetProjectInvestorQueryResponse>
    {
        public Guid ProjectId { get; set; }
        public string UserId { get; set; }
    }
}