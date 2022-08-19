using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectDocument
{
    public class GetProjectDocumentQueryHandler : IRequestHandler<GetProjectDocumentQuery, DocumentContentDTO>
    {
        private readonly IProjectDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public GetProjectDocumentQueryHandler(IProjectDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DocumentContentDTO> Handle(GetProjectDocumentQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.Projects.Include(p => p.Documents).ThenInclude(d => d.Type).AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            var type = await _context.DocumentTypes.AsNoTracking().FirstOrDefaultAsync(t => t.Key == request.Type, cancellationToken);
            if (type == null)
            {
                throw new DocumentTypeNotFoundException(request.Type);
            }
            var document = project.Documents.FirstOrDefault(d => d.Language == request.Language && d.Type.Key == request.Type);
            return new DocumentContentDTO
            {
                Language = request.Language,
                Type = _mapper.Map<DocumentTypesDTO>(type),
                Content = document?.Content,
                Id = document?.Id ?? Guid.Empty,
                ProjectId = project.Id
            };
        }
    }
}