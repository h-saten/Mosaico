using System.Threading.Tasks;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity.Abstractions
{
    public interface IKycService
    {
        Task SetUserVerifiedByIdAsync(string userId, string firstName = null, string lastName = null);
        Task SetUserVerifiedByEmailAsync(string email, string firstName = null, string lastName = null);
        Task SetUserDeclinedByIdAsync(string userId);
        Task SetUserDeclinedByEmailAsync(string email);
        Task SetUserDeclinedAsync(ApplicationUser user);
        Task SetUserVerifiedAsync(ApplicationUser user, string firstName = null, string lastName = null);
    }
}