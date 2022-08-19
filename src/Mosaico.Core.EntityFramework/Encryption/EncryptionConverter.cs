using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Mosaico.Core.EntityFramework.Encryption
{
    public class EncryptionConverter : ValueConverter<string, string>
    {
        private static string passwd;
        
        public EncryptionConverter(string passwd, ConverterMappingHints mappingHints = default)
            : base(EncryptExpr, DecryptExpr, mappingHints)
        {
            EncryptionConverter.passwd = passwd; 
        }

        private static Expression<Func<string, string>> DecryptExpr = x => !string.IsNullOrWhiteSpace(x) ? x.Decrypt(passwd) : string.Empty;
        static Expression<Func<string, string>> EncryptExpr = x => !string.IsNullOrWhiteSpace(x) ? x.Encrypt(passwd) : string.Empty;

    }
}