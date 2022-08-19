using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Serilog;

namespace Mosaico.Application.Wallet.Services
{
    public class MetamaskDeploymentEstimator : GasEstimatorBase
    {
        public MetamaskDeploymentEstimator(IWalletDbContext walletContext, IEthereumClientFactory ethereumClientFactory,
            ITokenService tokenService, ICrowdsaleService crowdsaleService, ILogger logger, IDaoService dao) : base(
            walletContext, ethereumClientFactory, tokenService, crowdsaleService, logger, dao)
        {
        }

        public override string PaymentMethod => Domain.Wallet.Constants.DeploymentPaymentMethods.Metamask;
    }
}