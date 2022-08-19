using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Base.Extensions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageForUpdate
{
    public class GetPageForUpdateQueryHandler : IRequestHandler<GetPageForUpdateQuery, UpdatePageDTO>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ICurrentUserContext _currentUser;
        private readonly ILogger _logger;

        public GetPageForUpdateQueryHandler(IProjectDbContext projectDbContext, ICurrentUserContext currentUser, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<UpdatePageDTO> Handle(GetPageForUpdateQuery request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages.FirstOrDefaultAsync(p => p.Id == request.Id,
                cancellationToken);
            
            if (page == null)
            {
                throw new PageNotFoundException(request.Id.ToString());
            }

            var socialMediaLinksTranslations = page.SocialMediaLinks.SelectMany(t => t.Translations);
            var groups = socialMediaLinksTranslations.GroupBy(g => g.Language)
                .ToDictionary(g => g.Key, grouping => grouping.Select(g => new SocialMediaLinkDTO
                {
                    Order = g.SocialMediaLink.Order,
                    Value = g.Value,
                    IsHidden = g.SocialMediaLink.IsHidden,
                    Key = g.SocialMediaLink.Key
                }).ToList());
            
            return new UpdatePageDTO
            {
                ShortDescription = page.ShortDescription?.ToDictionary(_currentUser.Language),
                PrimaryColor = page.PageCovers.FirstOrDefault()?.PrimaryColor,
                SecondaryColor = page.PageCovers.FirstOrDefault()?.SecondaryColor,
                CoverColor = page.PageCovers.FirstOrDefault()?.CoverColor,
                SocialMediaLinks = groups
            };
        }
    }
}