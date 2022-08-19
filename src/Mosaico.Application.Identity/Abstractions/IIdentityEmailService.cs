using System.Threading.Tasks;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Application.Identity.Abstractions
{
    public interface IIdentityEmailService
    {
        Task SendEmailConfirmationEmailAsync(ApplicationUser user, string callbackUrl, string language = Base.Constants.Languages.English);
        Task SendExternalUserConfirmationEmailAsync(ApplicationUser user, string callbackUrl, string language = Base.Constants.Languages.English);
        Task SendForgotPasswordEmailAsync(ApplicationUser user, string callbackUrl, string language = Base.Constants.Languages.English);
        Task SendPasswordChangeConfirmationCodeEmailAsync(string userEmail, string code, string language = Base.Constants.Languages.English);
        Task SendEmailChangeEmailAsync(string callbackUrl, string email, string language = Base.Constants.Languages.English);
        Task SendEmailChangedNotificationAsync(string email, string code, string language = Base.Constants.Languages.English);
        Task SendPasswordChangedNotificationAsync(string email, string callbackUrl, string language = Base.Constants.Languages.English);
        Task SendPhoneNumberChangedNotificationAsync(string email, string callbackUrl, string language = Base.Constants.Languages.English);
        Task SendDeviceAuthorizationCode(string email, string code, string language = Base.Constants.Languages.English);
        Task SendKycCompletedSuccessfullyAsync(string email, string language = Base.Constants.Languages.English);
    }
}