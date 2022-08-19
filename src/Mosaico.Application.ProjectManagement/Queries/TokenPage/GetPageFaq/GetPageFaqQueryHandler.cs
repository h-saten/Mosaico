using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageFaq
{
    public class GetPageFaqQueryHandler : IRequestHandler<GetPageFaqQuery, GetPageFaqQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;

        public GetPageFaqQueryHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<GetPageFaqQueryResponse> Handle(GetPageFaqQuery request, CancellationToken cancellationToken)
        {
            var page = await _projectDbContext.TokenPages.Include(t => t.Faqs).ThenInclude(f => f.Content).ThenInclude(c => c.Translations)
                .Include(t => t.Faqs).ThenInclude(f => f.Title).ThenInclude(t => t.Translations)
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == request.PageId, cancellationToken);
            
            if (page == null)
            {
                throw new PageNotFoundException(request.PageId.ToString());
            }

            if (string.IsNullOrWhiteSpace(request.Language))
            {
                request.Language = Mosaico.Base.Constants.Languages.English;
            }

            if (page.Faqs == null || !page.Faqs.Any())
            {
                return new GetPageFaqQueryResponse();
            }

            return new GetPageFaqQueryResponse
            {
                Faqs = page.Faqs.OrderBy(f => f.Order).Select(faq => new FaqDTO
                    {
                        Id = faq.Id,
                        Order = faq.Order,
                        IsHidden = faq.IsHidden,
                        Content = faq.Content.GetTranslationInLanguage(request.Language)?.Value,
                        Title = faq.Title.GetTranslationInLanguage(request.Language)?.Value
                    })
                    .ToList()
            };
        }
    }
}