using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetDocumentTypes
{
    public class GetDocumentTypesQueryHandler : IRequestHandler<GetDocumentTypesQuery, GetDocumentTypesQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IMapper _mapper;
        
        public GetDocumentTypesQueryHandler(IProjectDbContext projectDbContext, IMapper mapper, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetDocumentTypesQueryResponse> Handle(GetDocumentTypesQuery request, CancellationToken cancellationToken)
        {
            var documentTypes = await _projectDbContext.DocumentTypes.AsNoTracking().ToListAsync(cancellationToken);
            var dtos = documentTypes.Select(dt => _mapper.Map<DocumentTypesDTO>(dt)).ToList();

            return new GetDocumentTypesQueryResponse
            {
                Entities = dtos,
                Total = dtos.Count
            };
        }
    }
}