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

namespace Mosaico.Application.ProjectManagement.Commands.TokenPage.UpdateProjectIntroVideoUrl
{
    public class UpdateProjectIntroVideoUrlCommandHandler: IRequestHandler<UpdateProjectIntroVideoUrlCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        public UpdateProjectIntroVideoUrlCommandHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUser)
        {
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
        }

        public async Task<Guid> Handle(UpdateProjectIntroVideoUrlCommand request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages.Include(x => x.About).FirstOrDefaultAsync(f => f.Id == request.PageId, cancellationToken);
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            var videoUrlExist = await _projectDbContext.PageIntroVideos.FirstOrDefaultAsync(x => x.PageId == request.PageId);
            if (videoUrlExist != null)
            {
                videoUrlExist.VideoExternalLink = request.VideoUrl;
                videoUrlExist.ShowLocalVideo = request.ShowLocalVideo;
                _projectDbContext.PageIntroVideos.Update(videoUrlExist);
                await _projectDbContext.SaveChangesAsync(cancellationToken);

                return videoUrlExist.Id;
            }
            else
            {
                var pageIntroVideo = new PageIntroVideos()
                {
                    ShowLocalVideo = request.ShowLocalVideo,
                    VideoExternalLink = request.VideoUrl,
                    IsExternalLink = true,
                    PageId = request.PageId,
                    CreatedById = _currentUser.UserId
                };
                _projectDbContext.PageIntroVideos.Add(pageIntroVideo);

                await _projectDbContext.SaveChangesAsync(cancellationToken);

                return pageIntroVideo.Id;
            }
            
        }
    }
}
