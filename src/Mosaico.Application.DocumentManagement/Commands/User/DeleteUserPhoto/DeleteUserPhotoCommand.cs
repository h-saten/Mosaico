using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.User.DeleteUserPhoto
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class DeleteUserPhotoCommand : IRequest
    {
        public Guid UserId { get; set; }
    }
}