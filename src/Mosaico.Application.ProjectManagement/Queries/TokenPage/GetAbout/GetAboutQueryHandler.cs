using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetAbout
{
    public class GetAboutQueryHandler : IRequestHandler<GetAboutQuery, AboutDTO>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IMapper _mapper;

        public GetAboutQueryHandler(IProjectDbContext projectDbContext, IMapper mapper)
        {
            _projectDbContext = projectDbContext;
            _mapper = mapper;
        }

        public async Task<AboutDTO> Handle(GetAboutQuery request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages.Include(t => t.About)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.PageId, cancellationToken);
            
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            if (string.IsNullOrWhiteSpace(request.Language))
            {
                request.Language = Base.Constants.Languages.English;
            }

            if (page.About == null)
            {
                return new AboutDTO();
            }

            var aboutId = page.About.Id;
            
            var aboutContents = await _projectDbContext.AboutContentTranslations.Include(f => f.AboutContent).AsNoTracking()
                .Where(f => f.AboutContent.AboutId == aboutId && f.Language == request.Language)
                .FirstOrDefaultAsync(cancellationToken);
            if (aboutContents == null)
            {
                return new AboutDTO();
            }
            return new AboutDTO { Content = aboutContents.Value, Id = aboutId };
        }
    }
}