using System;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Entities.Staking;

namespace Mosaico.Application.DocumentManagement.Commands.Staking.UploadStakingTerms
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class UploadStakingTermsCommand : UploadDocumentRequestBase
    {
        public Guid TokenId { get; set; }
        public Guid StakingPairId { get; set; }
        public override Guid GetEntityId()
        {
            return StakingPairId;
        }
    }
}