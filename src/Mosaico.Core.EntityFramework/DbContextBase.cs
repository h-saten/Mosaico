using System.Collections.Generic;
using System.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mosaico.Core.EntityFramework.Abstractions;
using Mosaico.Core.EntityFramework.Encryption;
using Mosaico.Core.EntityFramework.Extensions;

namespace Mosaico.Core.EntityFramework
{
    public abstract class DbContextBase<TContext> : DbContext where TContext : DbContext
    {
        private const string pswd = "4@7V3UB}=F!,(svaH4J3ak3?YQ:A\"!^x.VBX@B|sz.X#iZ!c5_~3;-zr_'M6@ebZT.yX-\"(.8eV6LsnjM-&#xJaNy/gNzXR@Y])hkcMUY4t8[Q7gG{JX)|fk";
        private readonly IEnumerable<ISaveChangesCommandInterceptor> _interceptor;

        protected DbContextBase(DbContextOptions<TContext> options, IEnumerable<ISaveChangesCommandInterceptor> saveChangesCommandInterceptor = null):base(options)
        {
            _interceptor = saveChangesCommandInterceptor;
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyEncryptionValueConverter(pswd);
            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {       
            base.OnConfiguring(optionsBuilder);
            if(_interceptor != null)
            {
                optionsBuilder.AddInterceptors(_interceptor);
            }    
            optionsBuilder.UseLazyLoadingProxies();
        }
        
        public void RunMigration()
        {
            Database.Migrate();
        }
    }
}