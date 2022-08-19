using System;
using System.Threading.Tasks;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Domain.Identity.Abstractions
{
    public interface IPhoneNumberConfirmationCodesRepository
    {
        Task<PhoneNumberConfirmationCode> GetLastlyGeneratedCodeAsync(string userId);
        Task<PhoneNumberConfirmationCode> CreateCodeAsync(string userId, PhoneNumber phoneNumber, string codeValue = null);
        Task SetSecurityCodeUsed(Guid id);
    }
}