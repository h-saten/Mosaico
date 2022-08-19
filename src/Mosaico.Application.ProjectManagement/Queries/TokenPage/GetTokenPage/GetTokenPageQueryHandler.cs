using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs.TokenPage;
using Mosaico.Application.ProjectManagement.Extensions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.SDK.Wallet.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetTokenPage
{
    public class GetTokenPageQueryHandler : IRequestHandler<GetTokenPageQuery, PageDTO>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IWalletClient _walletClient;
        private readonly IMapper _mapper;

        public GetTokenPageQueryHandler(IProjectDbContext projectDbContext, IWalletClient walletClient, IMapper mapper, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _walletClient = walletClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PageDTO> Handle(GetTokenPageQuery request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages
                .Include(t => t.ShortDescription).ThenInclude(s => s.Translations)
                .Include(i => i.InvestmentPackages).ThenInclude(i => i.Translations)
                .Include(p => p.PageCovers).ThenInclude(p => p.Translations)
                .Include(p => p.SocialMediaLinks).ThenInclude(p => p.Translations)
                .Include(p => p.Faqs)
                .Include(p => p.Scripts)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
            
            if (page == null)
            {
                throw new PageNotFoundException(request.Id.ToString());
            }

            var articlesCount = await _projectDbContext.Articles.CountAsync(a => a.ProjectId == page.ProjectId, cancellationToken);
            var nftCount = await _walletClient.GetProjectNFTCountAsync(page.ProjectId);
            var reviewsCount = await _projectDbContext.PageReviews.CountAsync(p => p.PageId == page.Id, cancellationToken: cancellationToken);
            return new PageDTO
            {
                Id = page.Id,
                ShortDescription = page.ShortDescription?.GetTranslationInLanguage(request.Language)?.Value,
                InvestmentPackages = page.InvestmentPackages?.Select(p => p.ToFlatDTO(request.Language)).OrderBy(ip => ip.TokenAmount).ToList(),
                CoverUrl = page.PageCovers?.FirstOrDefault()?.GetTranslationInLanguage(request.Language)?.Value,
                PrimaryColor = page.PageCovers?.FirstOrDefault()?.PrimaryColor,
                SecondaryColor = page.PageCovers?.FirstOrDefault()?.SecondaryColor,
                CoverColor = page.PageCovers?.FirstOrDefault()?.CoverColor,
                HasInvestmentPackages = page.InvestmentPackages?.Any() ?? false,
                HasFAQs = page.Faqs.Any(),
                HasArticles = articlesCount > 0,
                HasNFTs = nftCount > 0,
                HasReviews = reviewsCount > 0,
                Scripts = page.Scripts.Select(s => _mapper.Map<ScriptDTO>(s)).ToList(),
                SocialMediaLinks = page.SocialMediaLinks.Where(s => !s.IsHidden).OrderBy(sm => sm.Order).Select(s => new SocialMediaLinkDTO
                {
                    Key = s.Key,
                    Order = s.Order,
                    Value = s.GetTranslationInLanguage(request.Language)?.Value,
                    IsHidden = s.IsHidden
                }).Where(d => !string.IsNullOrWhiteSpace(d.Value)).ToList()
            };
        }
    }
}