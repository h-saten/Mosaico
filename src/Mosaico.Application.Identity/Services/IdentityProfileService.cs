using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;

        public IdentityProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            var externalLogins = await _userManager.GetLoginsAsync(user);
            var hasKangaAccount = externalLogins != null && 
                                  externalLogins.Any(m => m.LoginProvider == "KangaExchange");
            
            if (hasKangaAccount)
            {
                var kangaUser = externalLogins.FirstOrDefault(p => p.LoginProvider == "KangaExchange");
                if (kangaUser != null)
                {
                    claims.Add(new Claim("KangaAccount", kangaUser.ProviderKey));
                }
            }

            if (user.NewsletterDataProcessingAgree)
            {
                claims.Add(new System.Security.Claims.Claim(
                    "NewsletterAgreement", 
                    user.NewsletterDataProcessingAgreedDate.ToString()));
            }
            
            if(!string.IsNullOrWhiteSpace(user.UserName))
                claims.Add(new Claim(JwtClaimTypes.PreferredUserName, user.UserName));
            if(!string.IsNullOrWhiteSpace(user.FirstName))
                claims.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName));
            if(!string.IsNullOrWhiteSpace(user.LastName))
                claims.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName));
            if (user.IsAdmin)
            {
                claims.Add(new Claim(JwtClaimTypes.Role, Authorization.Base.Constants.ScopesConstants.GlobalAdmin));
            }
            
            claims.Add(new Claim("kyc", (user.AMLStatus is AMLStatus.Confirmed or AMLStatus.KangaConfirmed).ToString()));
            
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
