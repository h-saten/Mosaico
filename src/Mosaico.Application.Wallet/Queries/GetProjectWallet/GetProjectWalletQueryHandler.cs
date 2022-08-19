using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Base.Extensions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.Wallet.Queries.GetProjectWallet
{
    public class GetProjectWalletQueryHandler : IRequestHandler<GetProjectWalletQuery, GetProjectWalletQueryResponse>
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IUserManagementClient _managementClient;

        public GetProjectWalletQueryHandler(IWalletDbContext walletDbContext, IUserManagementClient managementClient)
        {
            _walletDbContext = walletDbContext;
            _managementClient = managementClient;
        }

        public async Task<GetProjectWalletQueryResponse> Handle(GetProjectWalletQuery request, CancellationToken cancellationToken)
        {
            var projectWallet = await _walletDbContext.ProjectWallets.FirstOrDefaultAsync(pw => pw.ProjectId == request.ProjectId, cancellationToken);
            if (projectWallet == null)
            {
                return new GetProjectWalletQueryResponse
                {
                    TotalItems = 0,
                    TotalPages = 0,
                    Items = new List<ProjectWalletBalanceDTO>()
                };
            }
            
            var investors = await _walletDbContext.Investors.Where(i => i.ProjectId == request.ProjectId).OrderByDescending(i => i.TotalInvestment)
                .Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken: cancellationToken);
            if (!investors.Any()) return new GetProjectWalletQueryResponse();
            var userIds = investors.Select(i => i.UserId).ToList();
            var users = await _managementClient.GetUsersAsync(userIds, cancellationToken);
            var paymentCurrencies = await _walletDbContext.PaymentCurrencies.Where(pc => pc.Chain == projectWallet.Network).ToListAsync(cancellationToken);
            var response = new GetProjectWalletQueryResponse
            {
                Id = projectWallet.Id,
                ProjectId = projectWallet.ProjectId,
                TotalItems = await _walletDbContext.Investors.Where(i => i.ProjectId == request.ProjectId).CountAsync(cancellationToken)
            };
            foreach (var investor in investors)
            {
                var user = users.FirstOrDefault(u => string.Equals(u.Id, investor.UserId, StringComparison.InvariantCultureIgnoreCase));
                if (user != null)
                {
                    var wallet = investor.Balances.FirstOrDefault();
                    var dto = new ProjectWalletBalanceDTO
                    {
                        Address = wallet?.WalletAddress,
                        Id = investor.Id,
                        TotalInvestment = investor.TotalInvestment,
                        User = new ProjectInvestorDTO
                        {
                            Email = user.Email,
                            FullName = $"{user.FirstName} {user.LastName}".Trim(),
                            PhoneNumber = user.PhoneNumber,
                            UserId = user.Id
                        }
                    };
                    foreach (var investorBalance in investor.Balances)
                    {
                        var paymentCurrency = paymentCurrencies.FirstOrDefault(pc => pc.Ticker == investorBalance.Currency && pc.Chain == investorBalance.Network);
                        dto.Balances.Add(new TokenBalanceDTO
                        {
                            Balance = investorBalance.Balance.TruncateDecimals(),
                            Network = investorBalance.Network,
                            IsPaymentCurrency = true,
                            LogoUrl = paymentCurrency?.LogoUrl,
                            Symbol = investorBalance.Currency,
                            ContractAddress = paymentCurrency?.ContractAddress,
                            Name = paymentCurrency?.Name,
                            Currency = Constants.FIATCurrencies.USD,
                            Id = paymentCurrency?.Id ?? Guid.Empty
                        });
                        
                    }
                    response.Items.Add(dto);
                }
            }

            return response;
        }
    }
}