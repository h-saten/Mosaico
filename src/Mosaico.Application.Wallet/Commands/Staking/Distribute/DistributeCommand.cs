using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.Staking.Distribute
{
    [Restricted(nameof(CompanyId), Authorization.Base.Constants.Permissions.Company.CanEditDetails)]
    public class DistributeCommand : IRequest
    {
        public Guid StakingPairId { get; set; }
        public Guid CompanyId { get; set; }
        public decimal Amount { get; set; }
        public string UserId { get; set; }
    }
}