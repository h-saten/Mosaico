using System.Collections.Generic;

namespace Mosaico.Blockchain.Base
{
    public static class Constants
    {
        public static class BlockchainNetworks
        {
            public static readonly List<string> All = new() { Polygon, Ethereum, Ganache, Rinkeby, Mumbai };
            public const string Polygon = "Polygon";
            public const string Ethereum = "Ethereum";
            public const string Rinkeby = "Rinkeby";
            public const string Mumbai = "Mumbai";
            public const string Ganache = "Ganache";
            public const string Default = Polygon;
        }
        
        public static class ErrorCodes
        {
            public const string UnsupportedChain = "UNSUPPORTED_CHAIN";
            public const string UnsupportedContractVersion = "UNSUPPORTED_CONTRACT_VERSION";
        }
    }
}