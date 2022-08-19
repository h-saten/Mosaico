using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.Ratings;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("migrate-likes-form-investors")]
    public class MigrateLikesFromInvestorsCommand : CommandBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private Guid _projectId;

        public MigrateLikesFromInvestorsCommand(IWalletDbContext walletDbContext, IProjectDbContext projectDbContext, ILogger logger)
        {
            _walletDbContext = walletDbContext;
            _projectDbContext = projectDbContext;
            _logger = logger;
            SetOption("-projectId", "Project id", (s) => _projectId = Guid.Parse(s));
        }

        public override async Task Execute()
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(t => t.Id == _projectId);
            _logger.Information($"Starting migrating likes for project {project.Title}");
            var investors = await _walletDbContext.Transactions.Where(t =>
                    t.ProjectId == project.Id && t.UserId != null &&
                    t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed)
                .Select(t => t.UserId).Distinct().ToListAsync();
            _logger?.Information($"Found {investors.Count} unique investors for the project {project.Title}");
            foreach (var investor in investors)
            {
                if (!_projectDbContext.ProjectLikes.Any(t => t.UserId == investor && t.ProjectId == project.Id))
                {
                    _projectDbContext.ProjectLikes.Add(new ProjectLike
                    {
                        ProjectId = project.Id,
                        UserId = investor
                    });
                }
            }
            _logger?.Information($"Saving investors for {project.Title}");
            await _projectDbContext.SaveChangesAsync();
        }
    }
}