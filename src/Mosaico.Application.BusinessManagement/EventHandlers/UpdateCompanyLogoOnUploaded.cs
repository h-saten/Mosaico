using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Mosaico.Application.BusinessManagement.Queries.GetCompany;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.BusinessManagement;
using Serilog;

namespace Mosaico.Application.BusinessManagement.EventHandlers
{
    [EventInfo(nameof(UpdateCompanyLogoOnUploaded), "companies:api")]
    [EventTypeFilter(typeof(CompanyLogoUploaded))]
    public class UpdateCompanyLogoOnUploaded : EventHandlerBase
    {
        private readonly ILogger _logger;
        private readonly IBusinessDbContext _businessDb;
        private readonly ICacheClient _cacheClient;
        
        public UpdateCompanyLogoOnUploaded(ILogger logger, IBusinessDbContext businessDb, ICacheClient cacheClient)
        {
            _logger = logger;
            _businessDb = businessDb;
            _cacheClient = cacheClient;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var walletEvent = @event?.GetData<CompanyLogoUploaded>();
            if (walletEvent != null)
            {
                var company = await _businessDb.Companies.FirstOrDefaultAsync(p => p.Id == walletEvent.CompanyId);
                if (company == null)
                {
                    throw new CompanyNotFoundException(walletEvent.CompanyId);
                }
                _logger?.Verbose($"Company {walletEvent.CompanyId} was found. Attempting to change logo value to {walletEvent.LogoUrl}");
                company.LogoUrl = walletEvent.LogoUrl;
                await _businessDb.SaveChangesAsync();
                _logger?.Verbose($"Company logo was successfully changed");
                await _cacheClient.CleanAsync(new List<string>
                {
                    $"{nameof(GetCompanyQuery)}_{company.Id}",
                    $"{nameof(GetCompanyQuery)}_{company.Slug}"
                });
            }
        }
    }
}