using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Wallet.Commands.TokenManagement.DeleteTokenDistribution
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class DeleteTokenDistributionCommand : IRequest
    {
        public Guid TokenId { get; set; }
        public Guid TokenDistributionId { get; set; }
    }
}