using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("test-mtx", "tests meta transactions")]
    public class TestMetaTransactionsCommand : CommandBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly ITokenService _tokenService;
        private readonly IEthereumClientFactory _ethereumClientFactory;

        public TestMetaTransactionsCommand(IWalletDbContext walletDbContext, ITokenService tokenService, IEthereumClientFactory ethereumClientFactory)
        {
            _walletDbContext = walletDbContext;
            _tokenService = tokenService;
            _ethereumClientFactory = ethereumClientFactory;
        }

        public override async Task Execute()
        {
            // var projectWallet = await _walletDbContext.ProjectWalletAccounts.Include(pwa => pwa.ProjectWallet)
            //     .FirstOrDefaultAsync(w => w.Id == Guid.Parse("572AA582-EF90-4F29-F106-08DA1870672B"));
            // const string network = "Polygon";
            // var client = _ethereumClientFactory.GetClient(network);
            // var senderAccount = await client.GetAdminAccountAsync();
            // var ownerAccount = client.GetWalletAccount(projectWallet.ProjectWallet.Mnemonic, projectWallet.ProjectWallet.Password, projectWallet.AccountId);
            // var transaction = await _tokenService.TransferWithAuthorizationAsync(network, senderAccount, ownerAccount.PrivateKey,
            //     "0x2791Bca1f2de4661ED88A30C99A7a9449Aa84174", "0xFd079A89F6894Bc13c23b722D867a11F2dE7e25b", 5);
        }
    }
}