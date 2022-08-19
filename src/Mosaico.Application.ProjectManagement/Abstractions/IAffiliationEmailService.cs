using System.Collections.Generic;
using System.Threading.Tasks;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Abstractions
{
    public interface IAffiliationEmailService
    {
        Task SendInvestorAffiliationInvitationAsync(Project prj, string regulationLink, List<string> recipients, string language = Base.Constants.Languages.English);
    }
}