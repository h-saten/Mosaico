using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Storage.Base;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UploadCertificateBackground
{
    public class UploadCertificateBackgroundCommandHandler : IRequestHandler<UploadCertificateBackgroundCommand, Guid>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEventFactory _eventFactory;
        private readonly IStorageClient _storageClient;
        public UploadCertificateBackgroundCommandHandler(
            IStorageClient storageClient, 
            ILogger logger, 
            IProjectDbContext projectDbContext, 
            IEventPublisher eventPublisher,
            IEventFactory eventFactory)
        {
            _logger = logger;
            _projectDbContext = projectDbContext;
            _eventPublisher = eventPublisher;
            _eventFactory = eventFactory;
            _storageClient = storageClient;
        }

        public async Task<Guid> Handle(UploadCertificateBackgroundCommand request, CancellationToken cancellationToken)
        {
            _logger?.Verbose($"Attempting to create project document");
            
            var project = await _projectDbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            var investorCertificate = await _projectDbContext
                .InvestorCertificates
                .Where(x => x.ProjectId == project.Id)
                .SingleOrDefaultAsync(cancellationToken);

            if (investorCertificate is null)
            {
                investorCertificate = new InvestorCertificate(project.Id);
                await _projectDbContext.InvestorCertificates.AddAsync(investorCertificate, cancellationToken);
            }
            
            _logger?.Verbose($"Project {request.ProjectId} was found");

            var storageContainer = "ci-investor-certificates";
            var storageObject = new StorageObject
            {
                Container = storageContainer,
                Content = request.Content,
                Size = request.Content.Length,
                FileName = $"{Guid.NewGuid().ToString()}.{Path.GetExtension(request.OriginalFileName)}",
                MimeType = MimeTypes.GetMimeType(request.OriginalFileName)
            };
            var fileId = await _storageClient.CreateAsync(storageObject);
            var url = await _storageClient.GetFileURLAsync(fileId, storageContainer);

            var background = investorCertificate.AddBackground(request.Language, url);

            await _projectDbContext.SaveChangesAsync(cancellationToken);

            // await PublishEvents(background.Id, url);
            return background.Id;
        }

        private async Task PublishEvents(Guid documentId, string url)
        {
            var e = _eventFactory.CreateEvent(Events.ProjectManagement.Constants.EventPaths.Projects, new ProjectDocumentUploaded(documentId,url));
            await _eventPublisher.PublishAsync(e);
        }
    }
}