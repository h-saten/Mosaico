using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Base.Tools;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompaniesPublicly
{
    public class GetCompaniesPubliclyQueryHandler : IRequestHandler<GetCompaniesPubliclyQuery, GetCompaniesPubliclyQueryResponse>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUser;
        private readonly IProjectManagementClient _managementClient;
        private readonly IDateTimeProvider _provider;


        public GetCompaniesPubliclyQueryHandler(IBusinessDbContext context, ICurrentUserContext currentUser, IMapper mapper, IProjectManagementClient managementClient, IDateTimeProvider provider, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _managementClient = managementClient;
            _provider = provider;
            _logger = logger;
            _currentUser = currentUser;
        }

        public async Task<GetCompaniesPubliclyQueryResponse> Handle(GetCompaniesPubliclyQuery request, CancellationToken cancellationToken)
        {
            var dtos = new List<CompanyListDTO>();

            var companiesQuery = _context.Companies
                .Where(c => (request.Search == null || request.Search == "null" || c.CompanyName.Contains(request.Search)) && c.IsApproved);
            var count = await companiesQuery.AsNoTracking().CountAsync(cancellationToken);
            var items = await companiesQuery.Include(c => c.Proposals).AsNoTracking().Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);

            foreach (var item in items)
            {
                var dto = _mapper.Map<CompanyListDTO>(item);
                if (_currentUser.IsAuthenticated)
                {
                    dto.IsSubscribed = await _context.CompanySubscribers.AnyAsync(
                        c => c.CompanyId == item.Id && c.UserId == _currentUser.UserId, cancellationToken);
                }

                var companyProjects = await _managementClient.GetProjectsOfCompanyAsync(item.Id, 3, cancellationToken);
                dto.Projects = companyProjects.Select(cp => new CompanyListProjectDTO
                {
                    Id = cp.Id,
                    Title = cp.Title,
                    Slug = cp.Slug,
                    LogoUrl = cp.LogoUrl
                }).ToList();
                dto.TotalProposals = item.Proposals.Count;
                var now = _provider.Now();
                dto.OpenProposals = item.Proposals.Count(p => now >= p.StartsAt && now < p.EndsAt);

                dtos.Add(dto);
            }
            return new GetCompaniesPubliclyQueryResponse
            {
                Entities = dtos,
                Total = count
            };

            

        }
    }
}