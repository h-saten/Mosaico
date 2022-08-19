using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetTemplateContent
{
    public class GetTemplateContentQueryHandler : IRequestHandler<GetTemplateContentQuery, GetTemplateContentQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;
        private readonly IMapper _mapper;
        
        public GetTemplateContentQueryHandler(IProjectDbContext projectDbContext, IMapper mapper, ILogger logger = null)
        {
            _projectDbContext = projectDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetTemplateContentQueryResponse> Handle(GetTemplateContentQuery request, CancellationToken cancellationToken)
        {
            var template = await _projectDbContext.DocumentTemplates.AsNoTracking().FirstOrDefaultAsync(x => x.Key == request.Key && x.Language == request.Language, cancellationToken);

            if (template == null)
            {
                throw new TemplateNotFoundException(request.Key);
            }

            return new GetTemplateContentQueryResponse { Template = _mapper.Map<DocumentTemplateDTO>(template) };

        }
    }
}