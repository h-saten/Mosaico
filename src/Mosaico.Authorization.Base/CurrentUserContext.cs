using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Mosaico.Authorization.Base.Extensions;

namespace Mosaico.Authorization.Base
{
    public class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpContext;
        
        public CurrentUserContext(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
        }
        
        public bool IsGlobalAdmin => _httpContext.HttpContext != null && (_httpContext.HttpContext.User.IsInRole(Constants.ScopesConstants.GlobalAdmin)
                                     || _httpContext.HttpContext.User.HasClaim("client_act", "adminapi"));
        public bool IsAuthenticated => _httpContext.HttpContext?.User.Identity?.IsAuthenticated ?? false;
        public string Email => _httpContext.HttpContext?.GetClaimValue(JwtClaimTypes.Email);
        public string UserId => IsAuthenticated ? _httpContext.HttpContext.GetClaimValue(JwtClaimTypes.Subject) : string.Empty;

        public string Language => _httpContext.HttpContext.GetLanguage();
        
        public Task<string> GetAccessTokenAsync()
        {
            if (_httpContext.HttpContext != null)
            {
                return _httpContext.HttpContext.GetTokenAsync("access_token");
            }

            return Task.FromResult(string.Empty);
        }
    }
}