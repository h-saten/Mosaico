using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.User.UploadUserPhoto
{
    [Restricted(nameof(UserId), Authorization.Base.Constants.DefaultRoles.Self)]
    public class UploadUserPhotoCommand : IRequest<string>
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public Guid UserId { get; set; }
    }
}