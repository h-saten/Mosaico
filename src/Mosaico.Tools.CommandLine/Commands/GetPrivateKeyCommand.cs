using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("get-project-wallet-key")]
    public class GetPrivateKeyCommand : CommandBase
    {
        private string _userId;
        private string _network;
        private Guid _projectId;
        private readonly IEthereumClientFactory _ethereumClientFactory;
        private readonly ILogger _logger;
        private readonly IProjectWalletService _projectWalletService;
        private readonly IWalletDbContext _walletDbContext;

        public GetPrivateKeyCommand(ILogger logger, IProjectWalletService projectWalletService, IEthereumClientFactory ethereumClientFactory, IWalletDbContext walletDbContext)
        {
            _logger = logger;
            _projectWalletService = projectWalletService;
            _ethereumClientFactory = ethereumClientFactory;
            _walletDbContext = walletDbContext;
            SetOption("-userId", "user id", s => _userId = s);
            SetOption("-network", "network", s => _network = s);
            SetOption("-projectId", "project id", s => _projectId = Guid.Parse(s));
        }
        public override async Task Execute()
        {
            if (_projectId != Guid.Empty)
            {
                var projectWallet = await _projectWalletService.GetAccountAsync(_network, _projectId, _userId);
                if (projectWallet == null) throw new Exception("Project Wallet Not Found");
                _logger?.Information($"Mnemonic: {projectWallet.ProjectWallet.Mnemonic}");
                _logger?.Information($"Password: {projectWallet.ProjectWallet.Password}");
                _logger?.Information($"Account: {projectWallet.AccountId}");
                var client = _ethereumClientFactory.GetClient(_network);
                var account = client.GetWalletAccount(projectWallet.ProjectWallet.Mnemonic,
                    projectWallet.ProjectWallet.Password, projectWallet.AccountId);
                _logger?.Information($"Private Key: {account.PrivateKey}");
            }
            else
            {
                var wallets = await _walletDbContext.Wallets.Where(w => w.Network == _network && w.UserId == _userId).ToListAsync();
                foreach (var wallet in wallets)
                {
                    _logger?.Information($"Account: {wallet.AccountAddress}");
                    _logger?.Information($"Private key: {wallet.PrivateKey}");
                }
            }

            Console.ReadLine();
        }
    }
}