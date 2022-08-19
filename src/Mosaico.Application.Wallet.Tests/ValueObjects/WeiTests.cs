using System;
using System.Numerics;
using Mosaico.Domain.Wallet.ValueObjects;
using NUnit.Framework;

namespace Mosaico.Application.Wallet.Tests.ValueObjects
{
    public class WeiTests
    {
        
        [Test]
        public void ShouldReturnValidAmountForSpecifiedDecimalValues()
        {
            var testCases = new [] {1m, 2.35m, 0.0703m};
            var expectedCasesResults = new [] {new BigInteger(1), new BigInteger(235), new BigInteger(703)};
            for (var i = 0; i < testCases.Length; i++)
            {
                var wei = new Wei(testCases[i]); 
                Assert.IsTrue(wei.GetAmount().Equals(expectedCasesResults[i]));
            }
        }
        
        [Test]
        public void ShouldReturnValidDecimalPlacesForSpecifiedDecimalValues2()
        {
            var testCases = new [] {1m, 2.35m, 0.0703m, 13.429122275015857320m};
            var expectedCasesResults = new [] {0, 2, 4, 18};
            for (var i = 0; i < testCases.Length; i++)
            {
                var wei = new Wei(testCases[i]); 
                Assert.IsTrue(wei.GetDecimals().Equals(expectedCasesResults[i]));
            }
        }
        
        [Test]
        public void ShouldConvertProperlyFromWeiToDecimal()
        {
            decimal[] values = {1m, 2.35m, 0.0703m};
            
            foreach (var value in values)
            {
                var wei = new Wei(value);
                var decimalValue = wei.ToDecimal();
                Assert.AreEqual(0, decimal.Compare(value, decimalValue));
            }
        }
        
        [Test]
        public void ShouldReturnValidMultipliedValue()
        {
            var testCases1 = new [] {2m};
            var testCases2 = new [] {2.35m};
            var expectedCasesResults = new [] {new BigInteger(470)};
            for (var i = 0; i < testCases1.Length; i++)
            {
                var wei1 = new Wei(testCases1[i]);
                var wei2 = new Wei(testCases2[i]);
                var multiplied = wei1.Multiply(wei2);
                Assert.IsTrue(multiplied.GetAmount().Equals(expectedCasesResults[i]));
            }
        }
        
        [Test]
        public void ShouldAbortMultiplyByItself()
        {
            var wei = new Wei(2m);
            Assert.Throws<InvalidOperationException>(() => wei.Multiply(wei));
        }
    }
}