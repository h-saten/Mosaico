using System;
using System.Threading.Tasks;

namespace Mosaico.Authorization.Base
{
    public interface ICurrentUserContext
    {
        Task<string> GetAccessTokenAsync();
        bool IsGlobalAdmin { get; }
        bool IsAuthenticated { get; }
        string Email { get; }
        string UserId { get; }
        string Language { get; }
    }
}