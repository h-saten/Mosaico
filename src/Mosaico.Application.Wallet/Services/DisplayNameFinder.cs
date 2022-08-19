using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Services
{
    public class DisplayNameFinder : IDisplayNameFinder
    {
        private readonly Dictionary<string, string> _cache = new Dictionary<string, string>();
        private readonly IBusinessManagementClient _businessManagement;
        private readonly IUserManagementClient _managementClient;
        private readonly IWalletDbContext _walletDbContext;
        
        public DisplayNameFinder(IBusinessManagementClient businessManagement, IUserManagementClient managementClient, IWalletDbContext walletDbContext)
        {
            _businessManagement = businessManagement;
            _managementClient = managementClient;
            _walletDbContext = walletDbContext;
        }

        public async Task<string> FindDisplayNameAsync(string walletAddress, CancellationToken token = new CancellationToken())
        {
            if (_cache.ContainsKey(walletAddress))
            {
                return _cache[walletAddress];
            }

            var wallet = await _walletDbContext.Wallets.FirstOrDefaultAsync(w => w.AccountAddress == walletAddress, cancellationToken: token);
            if (wallet != null)
            {
                var user = await _managementClient.GetUserAsync(wallet.UserId, token);
                if (user != null)
                {
                    var value = user.Email;
                    if (!string.IsNullOrWhiteSpace(user.FirstName))
                    {
                        value = $"{user.FirstName} {user.LastName}";
                    }
                    _cache.Add(walletAddress, value);
                    return value;
                }
            }
                
            var companyWallet = await _walletDbContext.CompanyWallets.FirstOrDefaultAsync(w => w.AccountAddress == walletAddress, cancellationToken: token);
            if (companyWallet != null)
            {
                var company = await _businessManagement.GetCompanyAsync(companyWallet.CompanyId, token);
                if (company != null)
                {
                    _cache.Add(walletAddress, company.Name);
                    return company.Name;
                }
            }

            return string.Empty;
        }
    }
}