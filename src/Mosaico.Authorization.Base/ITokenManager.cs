using System.Threading.Tasks;

namespace Mosaico.Authorization.Base
{
    public interface ITokenManager
    {
        Task<bool> IsActiveAsync(string token);
        Task DeactivateAsync(string token);
    }
}
