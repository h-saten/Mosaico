using System;
using System.Threading.Tasks;
using Mosaico.Domain.Identity.Entities;

namespace Mosaico.Domain.Identity.Abstractions
{
    public interface ISecurityCodeRepository
    {
        Task<SecurityCode> GetCodeAsync(string userId, string context);
        Task<SecurityCode> CreateCodeAsync(string code, string userId, string context);
        Task SetSecurityCodeUsed(Guid id);
    }
}