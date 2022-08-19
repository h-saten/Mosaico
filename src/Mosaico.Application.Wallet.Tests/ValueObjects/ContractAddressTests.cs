using Mosaico.Domain.Wallet.ValueObjects;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.ValueObjects
{
    public class ContractAddressTests
    {
        
        [TestCase("0xC7c990547255A444B539CA39fAFf360910388E1a")]
        public void ShouldCreateValidObject(string given)
        {
            Assert.AreEqual(42, given.Length);
            new ContractAddress(given);
        }
    }
}