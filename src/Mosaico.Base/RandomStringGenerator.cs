using System;
using System.Security.Cryptography;
using Mosaico.Base.Abstractions;

namespace Mosaico.Base
{
    public class RandomStringGenerator : IStringGenerator
    {
        public string Generate(long size = 32)
        {
            using RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            var tokenData = new byte[size];
            rng.GetBytes(tokenData);
            var base64String = Convert.ToBase64String(tokenData);
            base64String = base64String.Replace("/", "a");
            return base64String;
        }
    }
}