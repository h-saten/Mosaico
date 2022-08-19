using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Entities;

namespace Mosaico.Application.Wallet.Services
{
    public class FeeService : IFeeService
    {
        private readonly IWalletDbContext _walletDbContext;

        public FeeService(IWalletDbContext walletDbContext)
        {
            _walletDbContext = walletDbContext;
        }

        public async Task<decimal> GetFeeAsync(Guid projectId, Guid? stageId = null)
        {
            var fee = default(FeeToProject);
            if(stageId.HasValue)
            {
                fee = await _walletDbContext.FeeToProjects.FirstOrDefaultAsync(f =>
                    f.ProjectId == projectId && f.StageId == stageId.Value);
            }

            fee ??= await _walletDbContext.FeeToProjects.FirstOrDefaultAsync(f => f.ProjectId == projectId);

            return fee?.FeePercentage ?? Domain.Wallet.Constants.StandardFee;
        }
        
        public decimal GetMosaicoFeeValueAsync(decimal feePercentage, decimal payedFee, decimal totalPayed)
        {
            return (totalPayed * feePercentage / 100) - payedFee;
        }
    }
}