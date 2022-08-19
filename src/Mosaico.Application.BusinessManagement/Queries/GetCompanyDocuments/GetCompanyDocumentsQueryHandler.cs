using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyDocuments
{
    internal class GetCompanyDocumentsQueryHandler : IRequestHandler<GetCompanyDocumentsQuery, GetCompanyDocumentsQueryResponse>
    {
        private readonly IBusinessDbContext _businessDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;

        public GetCompanyDocumentsQueryHandler(IBusinessDbContext businessDbContext, IMapper mapper, ICurrentUserContext currentUser, ILogger logger = null)
        {
            _businessDbContext = businessDbContext;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<GetCompanyDocumentsQueryResponse> Handle(GetCompanyDocumentsQuery request, CancellationToken cancellationToken)
        {
            var company = await _businessDbContext.Companies.Include(c => c.Documents).AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken);
            var companyDtos = company.Documents.Where(d => d.Language == _currentUser.Language).Select(d => new CompanyDocumentDTO()
            {
                Id = d?.Id,
                Language = d?.Language,
                Title = d?.Title,
                CompanyId = d?.CompanyId,
                Url = d?.Url,
            }).ToList();

            return new GetCompanyDocumentsQueryResponse { Documents = companyDtos };
        }
    }
}
