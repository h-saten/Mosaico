using System;
using System.Collections.Generic;
using System.Numerics;

namespace Mosaico.Domain.Wallet.ValueObjects
{
    public class Wei : ValueObject
    {
        private BigInteger Amount;
        private int DecimalsAmount;
        
        private Wei() {}
        
        public Wei(BigInteger amount, int decimals = 18)
        {
            Amount = amount;
            DecimalsAmount = decimals;
        }

        public Wei(decimal amount, int? decimals = 18)
        {
            int decimalPlaces = BitConverter.GetBytes(decimal.GetBits(amount)[3])[2];
            var amountBeforeLongCasting = amount * (decimal) Math.Pow(10, decimalPlaces);
            Amount = new BigInteger(amountBeforeLongCasting);
            DecimalsAmount = decimals is > 0 ? decimals.Value : decimalPlaces;
        }

        public BigInteger GetAmount()
        {
            return Amount;
        }

        // TODO probably to remove so that hide internal implementation
        public int GetDecimals()
        {
            return DecimalsAmount;
        }

        public decimal ToDecimal()
        {
            var divisor = new BigInteger(Math.Pow(10, DecimalsAmount));
            return (decimal) Amount /(decimal) divisor;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Amount;
        }

        public bool GreaterOrEqualTo(Wei value)
        {
            return Amount >= value.GetAmount();
        }

        public Wei Multiply(Wei value)
        {
            if (ReferenceEquals(this, value))
            {
                throw new InvalidOperationException();
            }
            
            var multipliedAmount = Amount * value.GetAmount();
            return new Wei(multipliedAmount);
        }

        public Wei Multiply(decimal value)
        {
            return Multiply(new Wei(value));
        }
    }
}