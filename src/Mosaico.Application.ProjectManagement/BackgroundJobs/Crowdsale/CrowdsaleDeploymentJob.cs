using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Domain.Wallet.Exceptions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Mosaico.Integration.SignalR.Abstractions;

namespace Mosaico.Application.ProjectManagement.BackgroundJobs.Crowdsale
{
    public class CrowdsaleDeploymentJob : ICrowdsaleDeploymentJob
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IWalletDbContext _walletContext;
        private readonly IIndex<string, ITokenService> _tokenServices;
        private readonly IIndex<string, ICrowdsaleService> _crowdsaleServices;
        private readonly ICrowdsaleDispatcher _crowdsaleDispatcher;
        

        public CrowdsaleDeploymentJob(IProjectDbContext projectDbContext, IIndex<string, ITokenService> tokenServices, IIndex<string, ICrowdsaleService> crowdsaleServices, IWalletDbContext walletContext, ICrowdsaleDispatcher crowdsaleDispatcher)
        {
            _projectDbContext = projectDbContext;
            _tokenServices = tokenServices;
            _crowdsaleServices = crowdsaleServices;
            _walletContext = walletContext;
            _crowdsaleDispatcher = crowdsaleDispatcher;
        }
        
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        public async Task DeployCrowdsaleAsync(Guid projectId, string userId)
        {
            try
            {
                var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);
                if (project == null)
                {
                    throw new ProjectNotFoundException(projectId);
                }

                if (!project.TokenId.HasValue)
                {
                    throw new TokenNotFoundException($"project {project.Id}");
                }

                var token = await _walletContext.Tokens.FirstOrDefaultAsync(t => t.Id == project.TokenId.Value);

                var companyWallet = await _walletContext.CompanyWallets
                    .FirstOrDefaultAsync(cw => cw.CompanyId == project.CompanyId.Value && cw.Network == token.Network);
                if (companyWallet == null)
                {
                    throw new CompanyWalletNotFoundException(project.CompanyId.ToString());
                }

                var tokenContractVersion = Integration.Blockchain.Ethereum.Constants.DefaultTokenContractVersion;
                if (!_tokenServices.TryGetValue(tokenContractVersion, out var tokenService))
                {
                    throw new UnknownContractVersionException("ERC20", tokenContractVersion);
                }

                var crowdsaleContractVersion =
                    Integration.Blockchain.Ethereum.Constants.DefaultCrowdsaleContractVersion;
                if (!_crowdsaleServices.TryGetValue(crowdsaleContractVersion, out var crowdsaleService))
                {
                    throw new UnknownContractVersionException("Crowdsale", crowdsaleContractVersion);
                }

                if (project.Crowdsale == null || string.IsNullOrWhiteSpace(project.Crowdsale.ContractAddress))
                {
                    var crowdsaleContract = await crowdsaleService.DeployAsync(token.Network, configuration =>
                    {
                        configuration.ERC20Address = token.Address;
                        configuration.StageCount = 10;
                        configuration.OwnerAddress = companyWallet.AccountAddress;
                        configuration.SoftCapDenominator = 30;
                        configuration.SupportedStableCoins = project.Crowdsale?.SupportedStableCoins?.ToList();
                        configuration.PrivateKey = companyWallet.PrivateKey;
                    });
                    
                    project.Crowdsale ??= new Domain.ProjectManagement.Entities.Crowdsale();
                    project.Crowdsale.Network = token.Network;
                    project.Crowdsale.ContractAddress = crowdsaleContract.ContractAddress;
                    project.Crowdsale.ContractVersion = crowdsaleContractVersion;
                    project.Crowdsale.OwnerAddress = crowdsaleContract.OwnerAddress;
                    await _projectDbContext.SaveChangesAsync();
                }

                var currentAllowance = await tokenService.GetAllowanceAsync(token.Network, x =>
                {
                    x.ContractAddress = token.Address;
                    x.OwnerPrivateKey = companyWallet.PrivateKey;
                    x.SpenderAddress = project.Crowdsale.ContractAddress;
                    x.OwnerAddress = companyWallet.AccountAddress;
                });
                if (project.Crowdsale.HardCap > currentAllowance)
                {
                    await tokenService.SetWalletAllowanceAsync(token.Network, x =>
                    {
                        x.Amount = project.Crowdsale.HardCap;
                        x.ContractAddress = token.Address;
                        x.Decimals = 18;
                        x.OwnerPrivateKey = companyWallet.PrivateKey;
                        x.SpenderAddress = project.Crowdsale.ContractAddress;
                    });
                }

                await _crowdsaleDispatcher.CrowdsaleDeployed(userId);
            }
            catch (Exception ex)
            {
                await _crowdsaleDispatcher.CrowdsaleDeploymentFailed(userId, ex.Message);
            }
        }
    }
}