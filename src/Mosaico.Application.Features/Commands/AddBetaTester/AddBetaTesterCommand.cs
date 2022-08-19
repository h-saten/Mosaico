using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.Features.Commands.AddBetaTester
{
    [Restricted(Authorization.Base.Constants.DefaultRoles.Admin)]
    public class AddBetaTesterCommand : IRequest<Guid>
    {
        public string UserId { get; set; }
    }
}