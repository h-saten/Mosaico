using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace Mosaico.Domain.Identity.ValueObjects
{
    public class EmailAddress : ValueObject
    {
        
        public string Address { get; private set; }

        private EmailAddress() {}

        public EmailAddress(string value)
        {
            if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(nameof(value));
            
            bool isValid = IsEmailValid(value);

            if (!isValid) throw new ArgumentException("invalid_email");

            Address = value.ToLower();
        }
        
        public bool IsEmailValid(string address)
        {
            try
            {
                MailAddress m = new MailAddress(address);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        public override string ToString()
        {
            return $"{Address}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            throw new NotImplementedException();
        }

    }
}
