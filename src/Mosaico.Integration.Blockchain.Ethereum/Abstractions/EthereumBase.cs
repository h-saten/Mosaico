using System;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Mosaico.Integration.Blockchain.Ethereum.Configuration;
using Mosaico.Integration.Blockchain.Ethereum.Exceptions;
using Nethereum.HdWallet;
using Nethereum.RPC.Accounts;
using Nethereum.Signer;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Mosaico.Integration.Blockchain.Ethereum.Abstractions
{
    public abstract class EthereumBase
    {
        public IAdminAccountProvider<EthereumAdminAccount> AdminAccountProvider { get; set; }
        public EthereumNetworkConfiguration Configuration { get; set; }
        
        protected readonly IValidator<EthereumNetworkConfiguration> ConfigValidator;

        protected EthereumBase(IValidator<EthereumNetworkConfiguration> configValidator = null)
        {
            ConfigValidator = configValidator;
        }
        
        public async Task<BigInteger> GetGasPriceAsync()
        {
            var client = GetClient();
            var gasPriceEstimate = await client.Eth.GasPrice.SendRequestAsync();
            return gasPriceEstimate.Value;
        }
        
        protected virtual Web3 GetClient()
        {
            if (ConfigValidator != null)
            {
                var result = ConfigValidator.Validate(Configuration);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors.FirstOrDefault()?.ErrorMessage ?? "Ethereum is misconfigured");
            }

            var web3 = new Web3(Configuration.Endpoint);
            web3.TransactionManager.UseLegacyAsDefault = true;
            return web3;
        }

        public virtual Web3 GetClient(IAccount account)
        {
            if (ConfigValidator != null)
            {
                var result = ConfigValidator.Validate(Configuration);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors.FirstOrDefault()?.ErrorMessage ?? "Ethereum is misconfigured");
            }

            var web3 = new Web3(account, Configuration.Endpoint);
            web3.TransactionManager.UseLegacyAsDefault = true;
            return web3;
        }
        
        public virtual Task<IAccount> GetAccountAsync(string privateKey, BigInteger chainId = default, int index = 0, CancellationToken token = new CancellationToken())
        {
            chainId = chainId == default ? GetChain(Configuration.Chain) : chainId;
            var account = new Account(privateKey, chainId);
            return Task.FromResult((IAccount)account);
        }
        
        public virtual async Task<IAccount> GetAdminAccountAsync(CancellationToken token = new CancellationToken())
        {
            try
            {
                var adminAccount = await AdminAccountProvider.GetAdminAccountDetailsAsync();
                var chainId = GetChain(Configuration.Chain);

                if (!string.IsNullOrWhiteSpace(adminAccount?.PrivateKey))
                    return await GetAccountAsync(adminAccount.PrivateKey, chainId, 0, token);

                if (!string.IsNullOrWhiteSpace(adminAccount?.Mnemonic) &&
                    !string.IsNullOrWhiteSpace(adminAccount?.Password) &&
                    !string.IsNullOrWhiteSpace(adminAccount?.AccountNumber))
                {
                    var wallet = new Wallet(adminAccount.Mnemonic, adminAccount.Password);

                    if (int.TryParse(adminAccount.AccountNumber, out var accountIndex))
                        return wallet.GetAccount(accountIndex, chainId);

                    return wallet.GetAccount(adminAccount.AccountNumber, 20, chainId);
                }

                throw new Exception("Admin account is misconfigured");
            }
            catch (Exception ex)
            {
                throw new EthereumAccountNotFoundException(
                    "Unable to identify admin account based on given configuration", ex);
            }
        }

        protected virtual BigInteger GetChain(string chainName)
        {
            if (Enum.TryParse<Chain>(chainName, out var chain))
                return new BigInteger((int)chain);
            if (BigInteger.TryParse(chainName, out var chainId))
            {
                return chainId;
            }
            throw new InvalidNetworkException(chainName);
        }

    }
}