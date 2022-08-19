using Microsoft.AspNetCore.SignalR;

namespace Mosaico.Integration.SignalR
{
    public class JwtUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("sub")?.Value!;
        }
    }
}