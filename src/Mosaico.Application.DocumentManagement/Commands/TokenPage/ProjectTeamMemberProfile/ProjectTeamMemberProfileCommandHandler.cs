using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Mosaico.Events.Base;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.ProjectTeamMemberProfile
{
    public class ProjectTeamMemberProfileCommandHandler : IRequestHandler<ProjectTeamMemberProfileCommand, string>
    {
        private readonly IDocumentDbContext _context;
        private readonly IEventFactory _eventFactory;
        private readonly IEventPublisher _eventPublisher;
        private readonly IStorageClient _storageClient;
        private readonly ILogger _logger;

        public ProjectTeamMemberProfileCommandHandler(IDocumentDbContext context, IEventFactory eventFactory, IEventPublisher eventPublisher, IStorageClient storageClient, ILogger logger = null)
        {
            _context = context;
            _eventFactory = eventFactory;
            _eventPublisher = eventPublisher;
            _storageClient = storageClient;
            _logger = logger;
        }
        
        public async Task<string> Handle(ProjectTeamMemberProfileCommand request, CancellationToken cancellationToken)
        {
            var container = Constants.Containers.ProjectTeamMemberProfileContainer;
            _logger?.Verbose($"Attempting to store {request.FileName} in container {container}");

            var fileId = await _storageClient.CreateAsync(new StorageObject
            {
                FileName = request.FileName,
                Container = container,
                Content = request.Content
            });

            _logger?.Verbose($"FIle {fileId} stored successfully");

            _logger?.Verbose($"Prepare to Profile URL");
            var profileUrl = await _storageClient.GetFileURLAsync(fileId, Constants.Containers.ProjectTeamMemberProfileContainer);
            _logger?.Verbose($"Profile URL is Prepared {profileUrl}");

            return profileUrl;
        }
    }
}
