using System;
using Mosaico.Core.EntityFramework;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.DataProtection
{
    public class DataProtectionDbContextFactory : DbContextFactoryBase<DataProtectionDbContext>, IDbFactory<DataProtectionDbContext>
    {
        protected override string DbSchema => Constants.Schemas.DataProtectionSchema;
        
        public override DataProtectionDbContext CreateDbContext(string[] args)
        {
            var builder = GetOptions();
            var context = (DataProtectionDbContext) Activator.CreateInstance(typeof(DataProtectionDbContext), builder.Options);
            return context;
        }
    }
}