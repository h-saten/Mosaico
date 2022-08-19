using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Mosaico.Base.Abstractions
{
    public interface ICertificateService
    {
        Task<X509Certificate2> GetCertificateAsync(string name, string password = null);
    }
}