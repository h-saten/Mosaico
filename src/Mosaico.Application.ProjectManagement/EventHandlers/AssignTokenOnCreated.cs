using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Mosaico.Events.Base;
using Mosaico.Events.Wallet;
using Serilog;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(AssignTokenOnCreated), "wallets:api")]
    [EventTypeFilter(typeof(TokenCreated))]
    public class AssignTokenOnCreated : EventHandlerBase
    {
        private readonly IProjectDbContext _dbContext;
        private readonly ILogger _logger;

        public AssignTokenOnCreated(IProjectDbContext dbContext, ILogger logger = null)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var data = @event?.GetData<TokenCreated>();
            if (data?.ProjectId != null)
            {
                var project = await _dbContext.GetProjectOrThrowAsync(data.ProjectId.Value);
                _logger?.Verbose($"Project {project.Id} was found. Updating token ID");
                project.TokenId = data.TokenId;
                await _dbContext.SaveChangesAsync();
                _logger?.Verbose($"Project was successfully updated with new token");
            }
        }
    }
}