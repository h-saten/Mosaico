using System;
using Microsoft.AspNetCore.Http;
using Mosaico.Base.Exceptions;

namespace Mosaico.Application.ProjectManagement.Exceptions.Affiliation
{
    public class PartnerAlreadyExistsException : ExceptionBase
    {
        public PartnerAlreadyExistsException(Guid id) : base($"Partner {id} already exists ")
        { 
        }

        public override string Code => "PARTNER_ALREADY_EXISTS";
        public override int StatusCode => StatusCodes.Status409Conflict;
    }
}