namespace Mosaico.Application.Identity.Queries.Authenticator.GetAuthenticatorKey
{
    public class GetAuthenticatorKeyQueryResponse
    {
        public string SharedKey { get; set; }
        public string AuthenticationUri { get; set; }
    }
}