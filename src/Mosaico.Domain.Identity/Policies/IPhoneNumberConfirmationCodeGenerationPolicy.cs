using System.Threading.Tasks;
using Mosaico.Domain.Identity.ValueObjects;

namespace Mosaico.Domain.Identity.Policies
{
    public interface IPhoneNumberConfirmationCodeGenerationPolicy
    {
        Task<bool> CanGenerate(string userId, PhoneNumber value);
    }
}