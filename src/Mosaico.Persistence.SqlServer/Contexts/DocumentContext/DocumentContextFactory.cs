using Mosaico.Core.EntityFramework;
using Mosaico.Persistence.SqlServer.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Persistence.SqlServer.Contexts.DocumentContext
{
    public class DocumentContextFactory : DbContextFactoryBase<DocumentContext>, IDbFactory<DocumentContext>
    {
        protected override string DbSchema => Domain.DocumentManagement.Constants.Schema;
    }
}
