using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mosaico.Core.EntityFramework;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.BusinessContext
{
    public class BusinessContextFactory : DbContextFactoryBase<BusinessContext>, IDbFactory<BusinessContext>
    {
        protected override string DbSchema => Domain.BusinessManagement.Constants.Schema;
    }
}