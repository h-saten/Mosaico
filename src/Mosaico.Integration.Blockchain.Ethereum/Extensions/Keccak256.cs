using Nethereum.Util;

namespace Mosaico.Integration.Blockchain.Ethereum.Extensions
{
    public static class Keccak256
    {
        public static string Keccak256Hash(this string value)
        {
            var result = new Sha3Keccack().CalculateHash(value);
            return "0x"+result;
        }
    }
}