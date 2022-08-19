using System;
using MediatR;

namespace Mosaico.Application.Identity.Commands.CreateExternalUser
{
    public class CreateExternalUserCommand : IRequest<Guid>
    {
        public string Email { get; set; }
    }
}