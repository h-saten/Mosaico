using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Autofac.Features.Indexed;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.CounterProviders;
using Mosaico.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.SignalR.Abstractions;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(SendInvitationOnProjectMemberAdded),  "projects:api")]
    [EventTypeFilter(typeof(ProjectMemberAddedEvent))]
    public class SendInvitationOnProjectMemberAdded : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IProjectEmailSender _emailService;
        private readonly IIndex<string, string> _urls;
        

        public SendInvitationOnProjectMemberAdded(IProjectDbContext projectDbContext, IProjectEmailSender emailService, IIndex<string, string> urls)
        {
            _projectDbContext = projectDbContext;
            _emailService = emailService;
            _urls = urls;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var eventData = @event?.GetData<ProjectMemberAddedEvent>();
            if (eventData != null)
            {
                var project = await _projectDbContext.Projects.Include(p => p.Members)
                    .FirstOrDefaultAsync(p => p.Id == eventData.ProjectId);
                var member = project?.Members?.FirstOrDefault(m => m.Email == eventData.Email);
                if (member != null)
                {
                    var encodedCode = HttpUtility.UrlEncode(member.AuthorizationCode);
                    var recipients = new List<string> { member.Email };

                    var baseUri = "/";
                    if (_urls.TryGetValue(Constants.UrlKeys.BaseUri, out var fetchedBaseUri))
                        baseUri = fetchedBaseUri;
                    var callbackUrl = $"{baseUri}/project/invitation?authorizationCode="+ encodedCode;

                    await _emailService.SendInvitationOnProjectMemberAddedAsync(project, callbackUrl, recipients);

                    member.IsInvitationSent = true;
                    _projectDbContext.ProjectMembers.Update(member);
                    await _projectDbContext.SaveChangesAsync();
                }
            }
        }
        
    }
}