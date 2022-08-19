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

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectPartners
{
    public class GetProjectPartnersQueryHandler : IRequestHandler<GetProjectPartnersQuery, GetProjectPartnersQueryResponse>
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IProjectDbContext _projectDbContext;

        public GetProjectPartnersQueryHandler(IMapper mapper, IProjectDbContext projectDbContext, ILogger logger = null)
        {
            _mapper = mapper;
            _logger = logger;
            _projectDbContext = projectDbContext;
        }

        public async Task<GetProjectPartnersQueryResponse> Handle(GetProjectPartnersQuery request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.TokenPages.Include(p => p.PagePartners)
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == request.PageId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.PageId);
            }

            //return new GetProjectPartnersQueryResponse
            //{
            //    Entities = project.PagePartners.Select(m => _mapper.Map<ProjectPartnerDTO>(m)).ToList()
            //};


            var projectPartnersQuery = _projectDbContext.PagePartners.AsQueryable()
                .Where(p => p.PageId == request.PageId).OrderBy(x => x.Order);
            var projectPartners = await projectPartnersQuery.AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
            var totalCount = await projectPartnersQuery.CountAsync(cancellationToken: cancellationToken);
            var dtos = projectPartners.Select(teamMember => _mapper.Map<ProjectPartnerDTO>(teamMember)).ToList();
            return new GetProjectPartnersQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }
    }
}