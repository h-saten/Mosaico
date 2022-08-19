using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Extensions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectStage
{
    public class GetProjectStageQueryHandler : IRequestHandler<GetProjectStageQuery, GetProjectStageQueryResponse>
    {
        private readonly IProjectDbContext _projectDb;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public GetProjectStageQueryHandler(IProjectDbContext projectDb, IMapper mapper, ILogger logger = null)
        {
            _projectDb = projectDb;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetProjectStageQueryResponse> Handle(GetProjectStageQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDb.GetProjectOrThrowAsync(request.ProjectId, cancellationToken);
            var stage= project.Stages.FirstOrDefault(s => s.Id == request.StageId);
            if (stage == null)
            {
                throw new StageNotFoundException(request.ProjectId, request.StageId);
            }

            return new GetProjectStageQueryResponse
            {
                Stage = _mapper.Map<StageDTO>(stage)
            };
        }
    }
}