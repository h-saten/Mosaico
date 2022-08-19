using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Affiliation
{
    public class PartnerNotFoundException : ExceptionBase
    {
        public PartnerNotFoundException(Guid partnerId) : base($"Partner {partnerId} not found")
        {
        }

        public override string Code => "PARTNER_NOT_FOUND";
        public override int StatusCode => StatusCodes.Status404NotFound;
    }
}