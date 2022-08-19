using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyDocument
{
    public class GetCompanyDocumentQueryHandler : IRequestHandler<GetCompanyDocumentQuery, DocumentContentDTO>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public GetCompanyDocumentQueryHandler(IBusinessDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DocumentContentDTO> Handle(GetCompanyDocumentQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Companies.Include(x => x.Documents).AsNoTracking().
                FirstOrDefaultAsync(x => x.Id == request.CompanyId, cancellationToken);
            var document = company.Documents.FirstOrDefault(d => d.Language == request.Language);
            return new DocumentContentDTO
            {
                Language = request.Language,
                Content = document?.Content,
                Id = document?.Id ?? Guid.Empty,
                CompanyId = company.Id,
            };
        }
    }
}
