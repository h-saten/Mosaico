using System;
using System.Linq;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Domain.Identity.Entities
{
    public class PhoneNumberConfirmationCode : EntityBase
    {
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public PhoneNumber PhoneNumber { get; set;}
        
        [Encrypted]
        public string Code { get; set;}
        public bool Used { get; private set;}
        public DateTimeOffset ExpiresAt { get; set; }
        
        private PhoneNumberConfirmationCode() {}
        
        public PhoneNumberConfirmationCode(string userId, PhoneNumber number, string code = null)
        {
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new MissingFieldException(userId);
            }

            UserId = userId;
            PhoneNumber = number;
            Code = !string.IsNullOrWhiteSpace(code) ? code : RandomCode(8);
            Used = false;
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(120);
        }

        public void MarkAsUsed()
        {
            Used = true;
        }
        
        private static string RandomCode(int size)  
        {  
            var random = new Random();
            
            const string chars = "abcdefghijklmnouprstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, size)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }  
    }
}