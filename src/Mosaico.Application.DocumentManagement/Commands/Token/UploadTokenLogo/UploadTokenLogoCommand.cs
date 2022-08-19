using System;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.DocumentManagement.Commands.Token.UploadTokenLogo
{
    [Restricted(nameof(TokenId), Authorization.Base.Constants.Permissions.Token.CanEdit)]
    public class UploadTokenLogoCommand : UploadDocumentRequestBase
    {
        public Guid TokenId { get; set; }
        
        public override Guid GetEntityId()
        {
            return TokenId;
        }
    }
}