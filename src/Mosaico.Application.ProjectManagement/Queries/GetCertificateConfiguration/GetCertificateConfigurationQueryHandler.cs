using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Models.CertificateGenerator;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetCertificateConfiguration
{
    public class GetCertificateConfigurationQueryHandler : IRequestHandler<GetCertificateConfigurationQuery, GetCertificateConfigurationResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
 
        public GetCertificateConfigurationQueryHandler(IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
        }

        public async Task<GetCertificateConfigurationResponse> Handle(GetCertificateConfigurationQuery request, CancellationToken cancellationToken)
        {
            var project =
                await _projectDbContext
                    .Projects
                    .Include(t => t.InvestorCertificate.Backgrounds)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(t => t.Id == request.ProjectId, cancellationToken);
            
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var backgroundExist = project.InvestorCertificate != null &&
                                  project.InvestorCertificate.GetBackgroundPath(request.Language) is not null;

            var response = new GetCertificateConfigurationResponse
            {
                Configuration = project.InvestorCertificate is not null
                    ? project.InvestorCertificate.GetConfiguration() 
                    : new DefaultCertificateConfiguration(),
                BackgroundUrl = backgroundExist ?
                    project.InvestorCertificate.GetBackgroundPath(request.Language)
                    : DefaultCertificateConfiguration.GetDefaultCertificateBackgroundUrl(request.Language),
                HasConfiguration = project.InvestorCertificate is not null,
                HasBlocksConfiguration = project.InvestorCertificate is not null,
                SendCertificateToInvestor = project.InvestorCertificate?.IsSendingEnabled() ?? false
            };

            return response;
        }
    }
}