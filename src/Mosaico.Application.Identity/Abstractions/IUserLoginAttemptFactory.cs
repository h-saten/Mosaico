using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity.Abstractions
{
    public interface IUserLoginAttemptFactory
    {
        Task<LoginAttempt> CreateUserLoginAttempt(ApplicationUser user, SignInResult result);
    }
}