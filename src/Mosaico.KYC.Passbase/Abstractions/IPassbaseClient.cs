using System.Threading.Tasks;
using Mosaico.KYC.Passbase.Models;

namespace Mosaico.KYC.Passbase.Abstractions
{
    public interface IPassbaseClient
    {
        Task<PassbaseIdentity> GetIdentityAsync(string id);
    }
}