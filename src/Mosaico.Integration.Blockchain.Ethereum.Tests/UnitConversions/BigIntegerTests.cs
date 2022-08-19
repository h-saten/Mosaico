using System;
using Nethereum.Util;
using NUnit.Framework;

namespace Mosaico.Integration.Blockchain.Ethereum.Tests.UnitConversions
{
    public class BigIntegerTests
    {
        [Test]
        [TestCase("1.0", ExpectedResult="1000000000000000000")]
        [TestCase("0.000450000000000010", ExpectedResult="450000000000010")]
        [TestCase("21.00045", ExpectedResult="21000450000000000000")]
        public string ShouldChangeDecimalValueToValidEthWeiRepresentation(string value)
        {
            var decimalValue = Decimal.Parse(value);
            var decimalToBigInteger = (new UnitConversion()).ToWei(decimalValue);
            return decimalToBigInteger.ToString();
        }
    }
}