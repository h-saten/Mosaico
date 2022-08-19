using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mosaico.Core.EntityFramework;
using Mosaico.Core.EntityFramework.Configurations;
using Mosaico.Persistence.SqlServer.Abstractions;

namespace Mosaico.Persistence.SqlServer.Contexts.ProjectContext
{
    public class ProjectContextFactory : DbContextFactoryBase<ProjectContext>, IDbFactory<ProjectContext>
    {
        protected override string DbSchema => Domain.ProjectManagement.Constants.Schema;
    }
}