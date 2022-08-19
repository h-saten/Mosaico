using System;

namespace Mosaico.Base.Tools
{
    public static class DecimalLongRepresentationExtension
    {
        public static long ConvertToLong(this decimal amount, int decimalPlaces = 18)
        {
            var multiplier = (decimal) Math.Pow(10, decimalPlaces);
            return Convert.ToInt64((long) amount * multiplier);
        }
    }
}