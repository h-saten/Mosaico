using System;
using System.Threading.Tasks;
using Mosaico.Application.Identity.Abstractions;

namespace Mosaico.Application.Identity.Services
{
    public class SecurityCodeGenerator : ISecurityCodeGenerator
    {
        public Task<string> GenerateSecurityCodeAsync()
        {
            var rnd = new Random();
            var rndVerificationDigit = rnd.Next(100000, 999999).ToString();
            return Task.FromResult(rndVerificationDigit.Insert(3, "-"));
        }
    }
}