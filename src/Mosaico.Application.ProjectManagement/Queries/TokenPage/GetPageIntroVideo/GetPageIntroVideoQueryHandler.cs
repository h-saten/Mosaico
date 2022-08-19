using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageIntroVideo
{
    public class GetPageIntroVideoQueryHandler: IRequestHandler<GetPageIntroVideoQuery, GetPageIntroVideoQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        public GetPageIntroVideoQueryHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<GetPageIntroVideoQueryResponse> Handle(GetPageIntroVideoQuery request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages.Include(t => t.PageIntroVideos).AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.PageId, cancellationToken);

            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            if (page.PageIntroVideos == null || !page.PageIntroVideos.Any())
            {
                return new GetPageIntroVideoQueryResponse();
            }

            return new GetPageIntroVideoQueryResponse()
            {
                introVideo = page.PageIntroVideos.Select(v => new IntroVideoDTO()
                {
                    Id = v.Id,
                    IsExternalLink = v.IsExternalLink,
                    ShowLocalVideo = v.ShowLocalVideo,
                    VideoUrl = v.VideoUrl,
                    VideoExternalLink = v.VideoExternalLink
                }).FirstOrDefault()
            };
        }
    }
}
