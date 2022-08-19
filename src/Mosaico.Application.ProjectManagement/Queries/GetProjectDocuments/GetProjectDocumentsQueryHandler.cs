using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectDocuments
{
    public class GetProjectDocumentsQueryHandler : IRequestHandler<GetProjectDocumentsQuery, GetProjectDocumentsQueryResponse>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetProjectDocumentsQueryHandler(IProjectDbContext projectDbContext, IMapper mapper, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetProjectDocumentsQueryResponse> Handle(GetProjectDocumentsQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.Include(p => p.Documents).ThenInclude(d => d.Type).AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            var documentTypes = await _projectDbContext.DocumentTypes.OrderBy(dt => dt.Order).AsNoTracking().ToListAsync(cancellationToken);
            var dtos = documentTypes.Select(dt =>
            {
                var doc = project.Documents.FirstOrDefault(t => t.Type.Id == dt.Id && t.Language == request.Language);
                return new DocumentDTO
                {
                    Id = doc?.Id,
                    Url = doc?.Url,
                    ProjectId = doc?.ProjectId,
                    Type = _mapper.Map<DocumentTypesDTO>(dt),
                    Language = string.IsNullOrWhiteSpace(doc?.Language) ? Base.Constants.Languages.English : doc.Language
                };
            }).ToList();
            
            return new GetProjectDocumentsQueryResponse
            {
                Documents = dtos
            };
        }
    }
}