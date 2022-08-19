using System.Data;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mosaico.Core.EntityFramework.Encryption;
using Mosaico.Domain.Identity.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.PersistedGrant
{
    public class IdentityPersistedGrantDbContext : PersistedGrantDbContext<IdentityPersistedGrantDbContext>
    {        
        private const string pswd = "4@7V3UB}=F!,(svaH4J3ak3?YQ:A\"!^x.VBX@B|sz.X#iZ!c5_~3;-zr_'M6@ebZT.yX-\"(.8eV6LsnjM-&#xJaNy/gNzXR@Y])hkcMUY4t8[Q7gG{JX)|fk";

        public IdentityPersistedGrantDbContext(DbContextOptions options, OperationalStoreOptions storeOptions) : base(options, storeOptions)
        {
        }

        public string Encrypt(string data)
        {
            return data.Encrypt(pswd);
        }

        public string Decrypt(string encryptedData)
        {
            return encryptedData.Decrypt(pswd);
        }

        public virtual IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            return Database.BeginTransaction(isolationLevel);
        }
        
        public void RunMigration()
        {
            Database.Migrate();
        }

        public string ContextName => "identity";
    }
}