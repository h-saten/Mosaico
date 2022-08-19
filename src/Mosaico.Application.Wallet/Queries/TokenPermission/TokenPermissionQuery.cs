using System;
using MediatR;
using Mosaico.Application.Wallet.Permissions;

namespace Mosaico.Application.Wallet.Queries.TokenPermission
{
    public class TokenPermissionQuery : IRequest<TokenPermissions>
    {
        public Guid TokenId { get; set; }
    }
}