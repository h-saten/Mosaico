using System.Threading.Tasks;
using Mosaico.Base.Abstractions;
using Mosaico.CommandLine.Base;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("migrate-database", "Migrates databases to new schema")]
    public class MigrateDatabaseCommand : CommandBase
    {
        private readonly IMigrationRunner _migrationRunner;
        private readonly ILogger _logger;
        private string _contextName;
        
        public MigrateDatabaseCommand(ILogger logger, IMigrationRunner migrationRunner)
        {
            _logger = logger;
            _migrationRunner = migrationRunner;
            SetOption("-contextName", "Context to run migrations on", (s) => _contextName = s);
        }

        public override Task Execute()
        {
            _logger.Information($"Starting migration of all registered contexts");
            _migrationRunner.RunMigrations(_contextName);
            _logger?.Information($"Migrations were successfully executed");
            return Task.CompletedTask;
        }
    }
}