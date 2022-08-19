using System;
using System.Text.Json.Serialization;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Identity.Commands.DeleteUser
{
    [Restricted(nameof(Id), Authorization.Base.Constants.DefaultRoles.Self)]
    public class DeleteUserCommand : IRequest
    {
        [JsonIgnore]
        public string Id { get; set; }

        public string Password { get; set; }
    }
}