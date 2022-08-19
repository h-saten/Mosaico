using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public interface IProjectEmailSender
    {
        Task SendInvestorInvitationsAsync(Project prj, List<string> recipients);
        Task SendInvitationOnProjectMemberAddedAsync(Project prj, string callbackUrl, List<string> email, string language = Base.Constants.Languages.English);
    }
}