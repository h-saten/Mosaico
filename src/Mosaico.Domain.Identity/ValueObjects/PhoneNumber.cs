using System;
using System.Collections.Generic;
using Mosaico.Domain.Identity.Exceptions;
using PhoneNumbers;

namespace Mosaico.Domain.Identity.ValueObjects
{
    public class PhoneNumber : ValueObject
    {
        public string Value  { get; set; }
        
        private PhoneNumber() {}

        public PhoneNumber(string phoneNumber)
        {
            if (String.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new EmptyPhoneNumberException();
            }
            
            var validatedNumber = ValidatePhoneNumber(phoneNumber);

            if (validatedNumber == null)
            {
                throw new InvalidPhoneNumberException(phoneNumber);
            }

            Value = phoneNumber;
        }

        private PhoneNumbers.PhoneNumber ValidatePhoneNumber(string phoneNumber)
        {
            var phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                var parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, null);
                var isValid = phoneNumberUtil.IsValidNumber(parsedPhoneNumber);

                if (isValid)
                {
                    return parsedPhoneNumber;
                }
            }
            catch (NumberParseException)
            {
                return null;
            }

            return null;
        }

        public override string ToString()
        {
            return $"{Value}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}