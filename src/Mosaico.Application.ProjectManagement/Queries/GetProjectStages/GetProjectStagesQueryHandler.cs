using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Extensions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectStages
{
    public class GetProjectStagesQueryHandler : IRequestHandler<GetProjectStagesQuery, GetProjectsStagesQueryResponse>
    {
        private readonly IProjectDbContext _projectDb;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetProjectStagesQueryHandler(IProjectDbContext projectDb, IMapper mapper, ILogger logger = null)
        {
            _projectDb = projectDb;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetProjectsStagesQueryResponse> Handle(GetProjectStagesQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDb.GetProjectOrThrowAsync(request.Id, cancellationToken);
            return new GetProjectsStagesQueryResponse
            {
                Stages = project.Stages.OrderBy(s => s.Order).Select(s => _mapper.Map<StageDTO>(s)).ToList()
            };
        }
    }
}