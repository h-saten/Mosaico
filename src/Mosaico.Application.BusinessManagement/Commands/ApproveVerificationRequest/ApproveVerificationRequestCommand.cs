using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.BusinessManagement.Commands.ApproveVerificationRequest
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class ApproveVerificationRequestCommand : IRequest<Guid>
    {
        public Guid CompanyId { get; set; }
    }
}