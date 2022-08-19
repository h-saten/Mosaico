using System.Security.Cryptography;
using System.Text;
using KangaExchange.SDK.Abstractions;
using Newtonsoft.Json;

namespace KangaExchange.SDK
{
    public class SignatureService : ISignatureService
    {
        public string GenerateSignature(object requestBody, string secretKey)
        {
            var bodyAsJson = JsonConvert.SerializeObject(requestBody);
            return ComputeSha512Hash(bodyAsJson, secretKey);
        }
        
        private string ComputeSha512Hash(string rawData , string key)
        {
            var encoding = new ASCIIEncoding();
            var keyBytes = encoding.GetBytes(key);
            // Create a SHA256   
            using var hashAlg = new HMACSHA512(keyBytes);
            // ComputeHash - returns byte array  
            var bytes = hashAlg.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            var builder = new StringBuilder();
            foreach (var t in bytes)
            {
                builder.Append(t.ToString("x2"));
            }
            return builder.ToString();
        }
    }
}