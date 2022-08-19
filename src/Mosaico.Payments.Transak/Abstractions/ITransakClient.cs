using System.Threading.Tasks;
using Mosaico.Payments.Transak.Models;

namespace Mosaico.Payments.Transak.Abstractions
{
    public interface ITransakClient
    {
        Task<TransakResponse<OrderDetails>> GetOrderDetailsAsync(string orderId);
    }
}