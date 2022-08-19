using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanySocialLinks
{
    public class GetCompanySocialLinksQueryHandler : IRequestHandler<GetCompanySocialLinksQuery, GetCompanySocialLinksQueryResponse>
    {
        private readonly IBusinessDbContext _context;

        public GetCompanySocialLinksQueryHandler(IBusinessDbContext context)
        {
            _context = context;
        }

        public async Task<GetCompanySocialLinksQueryResponse> Handle(GetCompanySocialLinksQuery request,
            CancellationToken cancellationToken)
        {
            var companySocialLinks = await _context.SocialMediaLinks.FirstOrDefaultAsync(s => s.CompanyId == request.CompanyId, cancellationToken: cancellationToken);
            return new GetCompanySocialLinksQueryResponse
            {
                Telegram = companySocialLinks?.Telegram,
                Youtube = companySocialLinks?.Youtube,
                LinkedIn = companySocialLinks?.LinkedIn,
                Facebook = companySocialLinks?.Facebook,
                Twitter = companySocialLinks?.Twitter,
                Instagram = companySocialLinks?.Instagram,
                Medium = companySocialLinks?.Medium
            };
        }
    }
}