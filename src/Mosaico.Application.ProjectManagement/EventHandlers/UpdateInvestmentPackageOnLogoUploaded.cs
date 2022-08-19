using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(UpdateInvestmentPackageOnLogoUploaded), "projects:api")]
    [EventTypeFilter(typeof(InvestmentPackageLogoUpdated))]
    public class UpdateInvestmentPackageOnLogoUploaded : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;

        public UpdateInvestmentPackageOnLogoUploaded(IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var eventData = @event?.GetData<InvestmentPackageLogoUpdated>();
            if (eventData != null)
            {
                var investmentPackage = await _projectDbContext.InvestmentPackages.FirstOrDefaultAsync(p => p.PageId == eventData.PageId && p.Id == eventData.PackageId);
                
                if (investmentPackage != null)
                {
                    investmentPackage.LogoUrl = eventData.Url;
                    await _projectDbContext.SaveChangesAsync();
                    _logger?.Verbose($"Investment package {eventData.PackageId} logo URL was successfully updated on page {eventData.PageId}");
                }
                else
                {
                    _logger?.Warning($"Investment package {eventData.PackageId} was not found on page {eventData.PageId}");
                }
            }
        }
    }
}