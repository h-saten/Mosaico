using System.Threading.Tasks;

namespace Mosaico.Application.Identity.Abstractions
{
    public interface ISecurityCodeGenerator
    {
        Task<string> GenerateSecurityCodeAsync();
    }
}