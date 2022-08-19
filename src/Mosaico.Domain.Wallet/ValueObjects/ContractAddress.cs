using System.Collections.Generic;
using System.Text.RegularExpressions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Domain.Wallet.ValueObjects
{
    public class ContractAddress : ValueObject
    {
        public readonly string Value;
        
        private ContractAddress() {}
        
        public ContractAddress(string address)
        {
            string pattern = @"^0x[a-fA-F0-9]{40}$"; 
            Regex regex = new Regex(pattern);  
            var isValid = regex.IsMatch(address);

            if (isValid is false)
                throw new InvalidContractAddress(address);
            
            Value = address;
        }
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}