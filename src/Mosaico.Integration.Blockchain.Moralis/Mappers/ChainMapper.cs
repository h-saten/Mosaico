namespace Mosaico.Integration.Blockchain.Moralis.Mappers
{
    internal static class ChainMapper
    {
        public static string Map(string value) =>
            value switch
            {
                Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Ethereum => "eth",
                Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Polygon => "polygon",
                _ => value
            };
    }
}