using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Transactions.UpdateProjectFee
{
    [Restricted(nameof(ProjectId), Authorization.Base.Constants.Permissions.Project.CanEditDetails)]
    public class UpdateProjectFeeCommand : IRequest
    {
        public Guid ProjectId { get; set; }
        public decimal FeePercentage { get; set; }
    }
}