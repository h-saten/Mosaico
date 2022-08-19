using Mosaico.Integration.Blockchain.Ethereum.Extensions;
using NUnit.Framework;

namespace Mosaico.Integration.Blockchain.Ethereum.Tests.Extensions
{
    public class JsonAbiFileTests
    {
        [Test]
        public void ReturnFileContent()
        {
            var content = JsonAbiFile.Read("Crowdsale/Events/TokensPurchased.json");
            Assert.NotNull(content);
        }
    }
}