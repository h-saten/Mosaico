using System.Threading.Tasks;
using KangaExchange.SDK.Models;
using KangaExchange.SDK.Models.Profile;

namespace KangaExchange.SDK.Abstractions
{
    public interface IKangaUserApiClient
    {
        Task<(string kangaUserId, string resetPasswordUrl)> CreateAccountAsync(string email, Language language);
        Task<(bool success, string errorCode)> PasswordRecoveryAsync(string email);
        Task<(bool success, UserProfileResponseDto, string errorCode)> UserProfile(string kangaUserId);
    }
}