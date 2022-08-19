using System.Threading.Tasks;
using Mosaico.SDK.Relay.Models;

namespace Mosaico.SDK.Relay.Abstractions
{
    public interface IStaking
    {
        Task<string> StakeAsync(StakeParams parameters);
        Task<string> ClaimAsync(ClaimParams parameters);
        Task<string> Withdraw(WithdrawParams parameters);
    }
}