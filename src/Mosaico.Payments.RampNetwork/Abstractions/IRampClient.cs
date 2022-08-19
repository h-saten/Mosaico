using System.Threading.Tasks;
using Mosaico.Payments.RampNetwork.Models;

namespace Mosaico.Payments.RampNetwork.Abstractions
{
    public interface IRampClient
    {
        Task<RampPurchase> GetPurchaseAsync(string id, string secret);
    }
}