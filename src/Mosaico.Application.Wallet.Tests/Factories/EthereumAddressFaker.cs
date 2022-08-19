using NBitcoin;

namespace Mosaico.Application.Wallet.Tests.Factories
{
    internal static class EthereumAddressFaker
    {
        public static string Generate()
        {
            var wallet = new Nethereum.HdWallet.Wallet(Wordlist.English, WordCount.Twelve);
            var account = wallet.GetAccount(0);
            return account.Address;
        }
    }
}