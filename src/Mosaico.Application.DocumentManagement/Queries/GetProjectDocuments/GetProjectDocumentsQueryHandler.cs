using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Domain.DocumentManagement.Abstractions;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Queries.GetProjectDocuments
{
    public class GetProjectDocumentsQueryHandler : IRequestHandler<GetProjectDocumentQuery, GetProjectDocumentsQueryResponse>
    {
        private readonly IDocumentDbContext _context;
        private readonly IMapper _mapper;

        public GetProjectDocumentsQueryHandler(IDocumentDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<GetProjectDocumentsQueryResponse> Handle(GetProjectDocumentQuery request, CancellationToken cancellationToken)
        {
            var documents = await _context.ProjectDocuments.AsNoTracking()
                .Where(pd => pd.ProjectId.Equals(request.ProjectId))
                .Select(pd => _mapper.Map<ProjectDocumentDTO>(pd))
                .ToListAsync(cancellationToken);


            foreach (var document in documents)
                document.Contents = await _context.DocumentContents
                    .Where(dc => dc.DocumentId.Equals(document.Id))
                    .Where(dc => string.IsNullOrEmpty(request.Language) || dc.Language == request.Language)
                    .Select(dc => _mapper.Map<DocumentContentDTO>(dc))
                    .ToListAsync(cancellationToken);

            return new GetProjectDocumentsQueryResponse { Documents = documents };
        }
    }
}
