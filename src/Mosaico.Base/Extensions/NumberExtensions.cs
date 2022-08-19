using System;

namespace Mosaico.Base.Extensions
{
    public static class NumberExtensions
    {
        public static decimal TruncateDecimals(this decimal number, int pow = 6)
        {
            var power = (decimal)Math.Pow(10, pow);
            return decimal.Truncate(number * power) / power;
        }
    }
}