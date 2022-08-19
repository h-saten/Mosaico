namespace Mosaico.Blockchain.Base.Extensions
{
    public static class SupportedBlockchainExtension
    {
        public static bool IsSupported(this string chain)
        {
            return Constants.BlockchainNetworks.All.Contains(chain);
        }
    }
}