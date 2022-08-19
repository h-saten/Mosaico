using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Integration.Blockchain.Ethereum.Models;
using Mosaico.Integration.Blockchain.Ethereum.Services.v1;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public interface IDaoService
    {
        public Task<string> DeployServiceAsync(string network, Action<DaoConfiguration> buildConfig = null);
        Task MintTokenAsync(string network, Daov1Configurations.MintTokenConfiguration config);
        Task BurnTokenAsync(string network, Daov1Configurations.BurnTokenConfiguration config);
        Task<string> CreateProposalAsync(string network, Daov1Configurations.CreateProposalConfiguration config);
        Task<string> VoteAsync(string network, Daov1Configurations.VoteConfiguration config);
        Task AddERC20Async(string network, Daov1Configurations.AddERC20Configuration config);
        Task<string> CreateERC20Async(string network, Daov1Configurations.CreateERC20Configuration config);
        Task<List<string>> GetTokensAsync(string network, string daoContract, string privateKey);
        Task<TransactionEstimate> EstimateDaoDeploymentAsync(string network, Action<DaoConfiguration> buildConfig = null);
        Task AddOwnerAsync(string network, string daoAddress, string ownerAddress, string privateKey);
        Task<ProposalState> GetStateAsync(string network, string address, string proposalId, string privateKey = null);
    }
}