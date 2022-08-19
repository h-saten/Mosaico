using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities.TokenPage;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Storage.Base;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UploadProjectIntroVideo
{
    public class UploadProjectIntroVideoCommandHandler: IRequestHandler<UploadProjectIntroVideoCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly IStorageClient _storageClient;
        public UploadProjectIntroVideoCommandHandler(IStorageClient storageClient, IProjectDbContext projectDbContext, ICurrentUserContext currentUser)
        {
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
            _storageClient = storageClient;
        }

        public async Task<Guid> Handle(UploadProjectIntroVideoCommand request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages.Include(x => x.About).FirstOrDefaultAsync(f => f.Id == request.PageId, cancellationToken);
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            var project = await _projectDbContext.Projects
                .FirstOrDefaultAsync(p => p.Id == page.ProjectId, cancellationToken);

            if (project == null)
            {
                throw new ProjectNotFoundException(page.ProjectId);
            }

            var url = "";

            if (request.Content.Length > 0)
            {
                var fileId = await _storageClient.CreateAsync(new StorageObject
                {
                    Container = "ci-documents",
                    Content = request.Content,
                    Size = request.Content.Length,
                    FileName = $"{project.Slug}/{project.Slug}_Intro.mp4",
                    MimeType = "video/mp4"
                }, false);
                url = await _storageClient.GetFileURLAsync(fileId, "ci-documents");
            }
            var videoExist = await _projectDbContext.PageIntroVideos.FirstOrDefaultAsync(x => x.PageId == request.PageId);
            if (videoExist != null)
            {
                videoExist.VideoUrl = url;
                videoExist.VideoExternalLink = request.VideoExternalLink;
                videoExist.ShowLocalVideo = request.ShowLocalVideo;
                _projectDbContext.PageIntroVideos.Update(videoExist);
                await _projectDbContext.SaveChangesAsync(cancellationToken);

                return videoExist.Id;
            }

            var pageIntroVideo = new PageIntroVideos()
            {
                ShowLocalVideo = request.ShowLocalVideo,
                VideoUrl = url,
                VideoExternalLink = request.VideoExternalLink,
                IsExternalLink = false,
                PageId = request.PageId,
                CreatedById = _currentUser.UserId
            };
            _projectDbContext.PageIntroVideos.Add(pageIntroVideo);

            await _projectDbContext.SaveChangesAsync(cancellationToken);

            return pageIntroVideo.Id;
        }
    }
}
