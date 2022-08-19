using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Permissions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.Wallet.Queries.TokenPermission
{
    public class TokenPermissionQueryHandler : IRequestHandler<TokenPermissionQuery, TokenPermissions>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenPermissionFactory _permissionFactory;
        private readonly ICurrentUserContext _currentUser;

        public TokenPermissionQueryHandler(IWalletDbContext walletDbContext, ITokenPermissionFactory permissionFactory, ICurrentUserContext currentUser)
        {
            _walletDbContext = walletDbContext;
            _permissionFactory = permissionFactory;
            _currentUser = currentUser;
        }

        public async Task<TokenPermissions> Handle(TokenPermissionQuery request, CancellationToken cancellationToken)
        {
            var token = await _walletDbContext.Tokens.FirstOrDefaultAsync(t => t.Id == request.TokenId, cancellationToken);
            if (token == null)
            {
                throw new TokenNotFoundException(request.TokenId);
            }
            
            var permissions = await _permissionFactory.GetTokenPermissionsAsync(token, _currentUser.UserId, cancellationToken);
            return permissions;
        }
    }
}