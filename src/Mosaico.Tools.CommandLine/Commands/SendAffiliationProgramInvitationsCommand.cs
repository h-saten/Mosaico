using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.CommandLine.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Tools.CommandLine.Commands
{
    [Command("invite-affiliation-program")]
    public class SendAffiliationProgramInvitationsCommand : CommandBase
    {
        private readonly IWalletDbContext _walletDbContext;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IIdentityContext _identityContext;
        private readonly IAffiliationEmailService _affiliationEmailService;
        private Guid _projectId;
        private readonly ILogger _logger;
        private string _regulationLink;

        public SendAffiliationProgramInvitationsCommand(IWalletDbContext walletDbContext, IProjectDbContext projectDbContext, IIdentityContext identityContext, IAffiliationEmailService affiliationEmailService, ILogger logger)
        {
            _walletDbContext = walletDbContext;
            _projectDbContext = projectDbContext;
            _identityContext = identityContext;
            _affiliationEmailService = affiliationEmailService;
            _logger = logger;
            SetOption("-projectId", "Project ID", (s) => _projectId = Guid.Parse(s));
            SetOption("-regulation", "Link to regulation file", (s) => _regulationLink = s);
        }

        public override async Task Execute()
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Id == _projectId);
            if(project == null) throw new ProjectNotFoundException(_projectId);
            _logger?.Information($"Project {project.Title} was found");
            var userIds = await _walletDbContext.Transactions.Where(t =>
                t.Status.Key == Domain.Wallet.Constants.TransactionStatuses.Confirmed && t.ProjectId == project.Id &&
                t.Type.Key == Domain.Wallet.Constants.TransactionType.Purchase).Select(t => t.UserId).Distinct().ToListAsync();
            if (!userIds.Any()) throw new Exception($"No users to notify");
            var emails = await _identityContext.Users.Where(u => userIds.Contains(u.Id)).Select(u => u.Email).ToListAsync();
            if (!emails.Any()) throw new Exception($"No emails were found");
            _logger?.Information($"About to notify {emails.Count} users about affiliation program");
            foreach (var email in emails)
            {
                await _affiliationEmailService.SendInvestorAffiliationInvitationAsync(project, _regulationLink, new List<string>
                {
                    email
                }, Base.Constants.Languages.Polish);
            }
            _logger?.Information($"Successfully notified everybody");
        }
    }
}