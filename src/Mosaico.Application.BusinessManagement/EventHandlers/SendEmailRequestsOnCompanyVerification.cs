using System.Linq;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Mosaico.Application.BusinessManagement.Abstractions;
using Mosaico.Application.BusinessManagement.Permissions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Mosaico.Integration.Email.Abstraction;
using Serilog;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(SendEmailRequestsOnCompanyVerification),  "companies:api")]
    [EventTypeFilter(typeof(CompanyVerificationCreatedEvent))]
    public class SendEmailRequestsOnCompanyVerification : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly ICompanyPermissionFactory _permissionFactory;
        private readonly ICompanyEmailSender _emailService;
        private readonly IIndex<string, string> _urls;

        public SendEmailRequestsOnCompanyVerification(ICompanyPermissionFactory permissionFactory, IIndex<string, string> urls, ICompanyEmailSender emailService, IEmailSender emailSender, ILogger logger = null)
        {
            _permissionFactory = permissionFactory;
            _logger = logger;
            _emailService = emailService;
            _urls = urls;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var companyEvent = @event.GetData<CompanyVerificationCreatedEvent>();
            if (companyEvent != null)
            {
                var baseUri = "/";
                if (_urls.TryGetValue(Constants.UrlKeys.BaseUri, out var fetchedBaseUri))
                    baseUri = fetchedBaseUri;
                var callbackUrl = $"{baseUri}/companies/verification?id=";
                await _emailService.SendEmailRequestsOnCompanyVerificationAsync(companyEvent.Title, companyEvent.Emails, callbackUrl);

            }
        }
    }
}