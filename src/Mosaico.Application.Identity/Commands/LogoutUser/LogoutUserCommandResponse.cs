namespace Mosaico.Application.Identity.Commands.LogoutUser
{
    public class LogoutUserCommandResponse
    {
        public string PostLogoutRedirectUri { get; set; }
        public string ClientName { get; set; }
        public string SignOutIframeUrl { get; set; }
    }
}