using System.Threading.Tasks;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.Integration.SignalR.Abstractions
{
    public interface ITokenPageDispatcher
    {
        Task DispatchCoverUpdatedAsync(string coverUrl);
        Task DispatchLogoUpdatedAsync(string logoUrl);
        Task DispatchPurchaseSuccessful(PurchaseSucceededDTO payload);
    }
}