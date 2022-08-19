using Mosaico.Integration.Blockchain.Ethereum.Extensions;
using NUnit.Framework;

namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Extensions
{
    public class Keccak256Tests
    {
        [TestCase("Transfer(address,address,uint256)", "0xddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef")]
        public void ReturnValidHash(string given, string expectedResult)
        {
            Assert.AreEqual(given.Keccak256Hash(), expectedResult);
        }
    }
}