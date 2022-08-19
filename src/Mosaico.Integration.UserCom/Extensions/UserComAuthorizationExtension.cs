using Mosaico.Integration.UserCom.Configurations;
using RestSharp;

namespace Mosaico.Integration.UserCom.Extensions
{
    internal static class UserComAuthorizationExtension
    {
        public static IRestRequest Authorize(this IRestRequest request, UserComConfig config)
        {
            return request.AddHeader(Constants.Parameters.Authorization, $"Token {config.AuthorizationToken}");
        }
    }
}