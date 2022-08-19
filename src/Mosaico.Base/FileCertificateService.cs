using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Mosaico.Base.Abstractions;

namespace Mosaico.Base
{
    public class FileCertificateService : ICertificateService
    {
        public Task<X509Certificate2> GetCertificateAsync(string name, string password)
        {
            var cer = new X509Certificate2(name, password);
            return Task.FromResult(cer);
        }
    }
}