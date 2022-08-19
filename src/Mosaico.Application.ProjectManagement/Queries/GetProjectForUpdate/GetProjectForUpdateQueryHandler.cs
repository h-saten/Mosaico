using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectForUpdate
{
    public class GetProjectForUpdateQueryHandler : IRequestHandler<GetProjectForUpdateQuery, UpdateProjectDTO>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IProjectDbContext _context;

        public GetProjectForUpdateQueryHandler(IMapper mapper, IProjectDbContext context, ILogger logger = null)
        {
            _mapper = mapper;
            _context = context;
            _logger = logger;
        }

        public async Task<UpdateProjectDTO> Handle(GetProjectForUpdateQuery request, CancellationToken cancellationToken)
        {
            var project = await _context.GetProjectOrThrowAsync(request.Id, cancellationToken);
            return _mapper.Map<UpdateProjectDTO>(project);
        }
    }
}