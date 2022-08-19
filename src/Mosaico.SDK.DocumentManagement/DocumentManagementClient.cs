using System;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.SDK.DocumentManagement.Abstractions;
using Mosaico.SDK.DocumentManagement.Models;
using Serilog;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.DocumentManagement.Exceptions;

namespace Mosaico.SDK.DocumentManagement
{
    public class DocumentManagementClient : IDocumentManagementClient
    {
        private readonly IDocumentDbContext _context;
        private readonly ILogger _logger;

        public DocumentManagementClient(IDocumentDbContext context, ILogger logger = null)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<CompanyLogo> GetCompanyLogo(Guid companyId, CancellationToken token = new CancellationToken())
        {
            var companyLogo = await _context.CompanyLogos.FirstOrDefaultAsync(p => p.Id == companyId, cancellationToken: token);
            /*
             uncomment when the path of getting company logo is agreed
            {
                throw new LogoNotFoundException($"Logo for company {companyId} not found");
            }
            */
            if (companyLogo == null)
                return default;
            
            return new CompanyLogo
            {
                CompanyId = companyLogo.Id,
                CompanyLogoUrl = companyLogo.Title //verify what is the source the URL is coming from
            };
        }
    }
}