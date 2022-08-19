using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Application.Identity.Queries.GetUserProfilePermissions
{
    public class GetUserProfilePermissionsQueryHandler : IRequestHandler<GetUserProfilePermissionsQuery, GetUserProfilePermissionsQueryResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserContext _currentUser;
        private readonly IIdentityContext _context;

        public GetUserProfilePermissionsQueryHandler(UserManager<ApplicationUser> userManager, ICurrentUserContext currentUser, IIdentityContext context)
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<GetUserProfilePermissionsQueryResponse> Handle(GetUserProfilePermissionsQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new UserNotFoundException(request.UserId);
            }

            var logins = await _userManager.GetLoginsAsync(user);
            var verifications = await _context.KycVerifications.CountAsync(u => u.UserId == request.UserId, cancellationToken);

            return new GetUserProfilePermissionsQueryResponse
            {
                {Authorization.Base.Constants.Permissions.UserProfile.CanRead, string.Equals(_currentUser.UserId, request.UserId, StringComparison.InvariantCultureIgnoreCase) || _currentUser.IsGlobalAdmin},
                {Authorization.Base.Constants.Permissions.UserProfile.CanEditPassword, !logins.Any() && string.Equals(_currentUser.UserId, request.UserId, StringComparison.InvariantCultureIgnoreCase)},
                {Authorization.Base.Constants.Permissions.UserProfile.CanEditEmail, !logins.Any() && string.Equals(_currentUser.UserId, request.UserId, StringComparison.InvariantCultureIgnoreCase)},
                {Authorization.Base.Constants.Permissions.UserProfile.CanEditProfile, string.Equals(_currentUser.UserId, request.UserId, StringComparison.InvariantCultureIgnoreCase) || _currentUser.IsGlobalAdmin},
                {Authorization.Base.Constants.Permissions.UserProfile.CanEditPhone, string.Equals(_currentUser.UserId, request.UserId, StringComparison.InvariantCultureIgnoreCase)},
                {Authorization.Base.Constants.Permissions.UserProfile.CanVerifyAccount, string.Equals(_currentUser.UserId, request.UserId, StringComparison.InvariantCultureIgnoreCase) && 
                    (user.AMLStatus != AMLStatus.Confirmed && user.AMLStatus != AMLStatus.KangaConfirmed) && !user.IsAMLVerificationDisabled && verifications <= 3}
            };
        }
    }
}