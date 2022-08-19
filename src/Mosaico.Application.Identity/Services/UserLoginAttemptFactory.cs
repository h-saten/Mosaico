using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity.Services
{
    public class UserLoginAttemptFactory : IUserLoginAttemptFactory
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLoginAttemptFactory(IHttpContextAccessor httpContextAccessor = null)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<LoginAttempt> CreateUserLoginAttempt(ApplicationUser user, SignInResult result)
        {
            if (_httpContextAccessor?.HttpContext != null)
            {
                var attempt = new LoginAttempt
                {
                    UserId = user?.Id,
                    User = user,
                    Successful = result.Succeeded,
                    LoggedInAt = DateTimeOffset.UtcNow
                };
                if (_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("User-Agent", out var agentInfo))
                {
                    attempt.AgentInfo = agentInfo;
                }

                attempt.IP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();

                return Task.FromResult(attempt);
            }

            return Task.FromResult(default(LoginAttempt));
        }
    }
}