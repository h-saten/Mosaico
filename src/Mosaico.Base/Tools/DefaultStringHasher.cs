using System.Security.Cryptography;
using System.Text;
using Mosaico.Base.Abstractions;

namespace Mosaico.Base.Tools
{
    public class DefaultStringHasher : IStringHasher
    {
        public string CreateHash(string data)
        {
            var hasher = new SHA512CryptoServiceProvider();
            var hashBytes = hasher.ComputeHash(Encoding.UTF8.GetBytes(data));
            return Encoding.UTF8.GetString(hashBytes);
        }

        public bool IsHashValid(string data, string hash)
        {
            var verificationHash = CreateHash(data);
            return string.Equals(hash, verificationHash);
        }
    }
}