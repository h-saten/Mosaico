using System.Threading;
using System.Threading.Tasks;
using Mosaico.Integration.UserCom.Models;

namespace Mosaico.Integration.UserCom.Abstractions
{
    public interface IUserComUsersApiClient
    {
        Task<FindUserResponse> FindUserByEmailAsync(string email, CancellationToken token = new());
        Task<CreateUserResponse> CreateUserAsync(string email, string name, CancellationToken token = new());
    }
}