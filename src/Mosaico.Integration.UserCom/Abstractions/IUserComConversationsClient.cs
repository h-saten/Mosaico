using System.Threading;
using System.Threading.Tasks;
using Mosaico.Integration.UserCom.Models.Request;

namespace Mosaico.Integration.UserCom.Abstractions
{
    public interface IUserComConversationsClient
    {
        Task CreateConversationAsync(CreateConversationParams parameters, CancellationToken t = new());
    }
}