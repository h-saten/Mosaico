using System.Collections.Generic;
using System.Linq;
using Mosaico.Base.Abstractions;
using Mosaico.Core.EntityFramework;
using Serilog;

namespace Mosaico.Persistence.SqlServer
{
    public class MigrationRunner : IMigrationRunner
    {
        private readonly IEnumerable<IDbContext> _contexts;
        private readonly ILogger _logger;

        public MigrationRunner(ILogger logger = null, IEnumerable<IDbContext> contexts = null)
        {
            _logger = logger;
            _contexts = contexts;
        }

        public void RunMigrations(string contextName = null)
        {
            if (_contexts != null)
            {
                var contextsToExecute = _contexts;
                if (!string.IsNullOrWhiteSpace(contextName))
                {
                    contextsToExecute = _contexts.Where(c => c.ContextName == contextName || string.IsNullOrWhiteSpace(c.ContextName));
                }
                
                foreach (var context in contextsToExecute)
                {
                    _logger?.Information($"Attempting to run migrations for {context.GetType().FullName}");
                    context.RunMigration();
                    _logger?.Information($"Migrations for context {context.GetType().FullName} were successfully applied");
                }
            }
        }
    }
}