using System;
using System.Linq;
using Mosaico.Core.EntityFramework.Attributes;
using Mosaico.Domain.Base;
using Mosaico.Domain.Identity.Exceptions;

namespace Mosaico.Domain.Identity.Entities
{
    public class SecurityCode : EntityBase
    {
        public static readonly string DeviceAuthorizationContext = "DEVICE_AUTHORIZATION";
        
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        
        public DateTimeOffset ExpiresAt { get; set; }
        
        [Encrypted]
        public string Code { get; set; }
        public string Context { get; set; }
        public bool IsUsed { get; set; }

        public SecurityCode() {}
        
        
        public SecurityCode(string userId, DateTimeOffset? expireAt, string code = null, string context = null)
        {
            if (String.IsNullOrWhiteSpace(code))
            {
                throw new InvalidConfirmationCodeException(code);
            }
            if (String.IsNullOrWhiteSpace(userId))
            {
                throw new MissingFieldException(userId);
            }

            UserId = userId;
            Code = string.IsNullOrWhiteSpace(code) ? code : RandomCode(8);
            ExpiresAt = expireAt ?? DateTimeOffset.UtcNow.AddMinutes(Constants.SecurityCodeExpiresInMinutes);
            IsUsed = false;
            Context = context;
        }

        public static string RandomCode(int size)  
        {  
            var random = new Random();
            
            const string chars = "abcdefghijklmnouprstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, size)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }  

        public void MarkAsUsed()
        {
            IsUsed = true;
        }
    }
}