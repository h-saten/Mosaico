using System.Threading.Tasks;
using Mosaico.SDK.Relay.Models;

namespace Mosaico.SDK.Relay.Abstractions
{
    public interface IToken
    {
        Task<decimal> AllowanceAsync(AllowanceParams parameters);
    }
}