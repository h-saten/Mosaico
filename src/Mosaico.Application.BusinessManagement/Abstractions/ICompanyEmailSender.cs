using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mosaico.Application.BusinessManagement.Abstractions
{
    public interface ICompanyEmailSender
    {
        Task SendEmailRequestsOnCompanyVerificationAsync(string title, List<string> recipients, string callbackUrl, string language = Base.Constants.Languages.English);
    }
}
