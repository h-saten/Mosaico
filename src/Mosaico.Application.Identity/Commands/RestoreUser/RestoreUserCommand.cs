using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.RestoreUser
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class RestoreUserCommand : IRequest
    {
        public string Id { get; set; }

    }
}