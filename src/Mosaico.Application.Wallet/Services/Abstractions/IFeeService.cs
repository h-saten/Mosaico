using System;
using System.Threading.Tasks;

namespace Mosaico.Application.Wallet.Services.Abstractions
{
    public interface IFeeService
    {
        Task<decimal> GetFeeAsync(Guid projectId, Guid? stageId = null);
        decimal GetMosaicoFeeValueAsync(decimal feePercentage, decimal payedFee, decimal totalPayed);
    }
}