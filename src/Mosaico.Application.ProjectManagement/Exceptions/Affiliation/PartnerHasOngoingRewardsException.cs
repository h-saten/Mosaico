using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Affiliation
{
    public class PartnerHasOngoingRewardsException : ExceptionBase
    {
        public PartnerHasOngoingRewardsException(Guid partnerId) : base($"Partner {partnerId} has unclaimed rewards")
        {
        }

        public override string Code => "PARTNER_HAS_UNCLAIMED_REWARDS";
        public override int StatusCode => StatusCodes.Status400BadRequest;
    }
}