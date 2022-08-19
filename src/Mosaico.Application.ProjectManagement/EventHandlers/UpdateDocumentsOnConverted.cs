using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(UpdateDocumentsOnConverted), "projects:api")]
    [EventTypeFilter(typeof(ProjectDocumentConverted))]
    public class UpdateDocumentsOnConverted : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;


        public UpdateDocumentsOnConverted(IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var projectDocumentEvent = @event?.GetData<ProjectDocumentConverted>();
            if (projectDocumentEvent != null)
            {
                var document = await _projectDbContext.Documents.FirstOrDefaultAsync(x=>x.Id == projectDocumentEvent.DocumentId);
                if (document != null)
                {
                    _logger?.Verbose($"Project document {projectDocumentEvent.DocumentId} was found. Attempting to change document URL value to {projectDocumentEvent.Url}");
                    document.Url = projectDocumentEvent.Url;
                    await _projectDbContext.SaveChangesAsync();
                    _logger?.Verbose($"Project document URL was successfully changed");
                } else
                {
                    throw new DocumentTypeNotFoundException("");
                }
            }
        }
    }
}