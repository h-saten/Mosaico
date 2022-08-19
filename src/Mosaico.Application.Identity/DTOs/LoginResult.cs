namespace Mosaico.Application.Identity.DTOs
{
    public class LoginResult
    {
        public LoginResponseTypeDTO Type { get; set; }
        public string DefaultRedirect { get; set; }
        public string RedirectAfterLoginUrl { get; set; }
        public bool DeviceVerificationRequired { get; set; }
        public bool IsAuthenticatorEnabled { get; set; }
        public DeviceAuthorizationDto DeviceAuthorization { get; set; }

        public LoginResult(LoginResponseTypeDTO type, string defaultRedirect, string redirectAfterLoginUrl)
        {
            Type = type;
            DefaultRedirect = defaultRedirect;
            RedirectAfterLoginUrl = redirectAfterLoginUrl;
        }
    }
}