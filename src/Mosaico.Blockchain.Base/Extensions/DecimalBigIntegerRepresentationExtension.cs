using System;
using System.Numerics;

namespace Mosaico.Blockchain.Base.Extensions
{
    public static class DecimalBigIntegerRepresentationExtension
    {
        public static BigInteger ConvertToBigInteger(this decimal amount, int decimalPlaces = 18)
        {
            var multiplier = (decimal) Math.Pow(10, decimalPlaces);
            return new BigInteger(amount * multiplier);
        }

        public static decimal ConvertToDecimal(this BigInteger num, int decimalPlaces = 18)
        {
            var divisor = new BigInteger(Math.Pow(10, decimalPlaces));
            return (decimal) num / (decimal) divisor;
        }
    }
}