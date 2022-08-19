using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.InitKycVerification
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class InitKycVerificationCommand : IRequest<Guid>
    {
        public string Id { get; set; }
        public string Provider { get; set; }
        public string UserId { get; set; }
    }
}