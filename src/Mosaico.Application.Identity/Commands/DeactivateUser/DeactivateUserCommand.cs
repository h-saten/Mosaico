using MediatR;
using Mosaico.Authorization.Base;
using System;

namespace Mosaico.Application.Identity.Commands.DeactivateUser
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class DeactivateUserCommand : IRequest
    {
        public string Id { get; set; }
        public bool Status { get; set; }
        public string Reason { get; set; } = "NA";
    }
}