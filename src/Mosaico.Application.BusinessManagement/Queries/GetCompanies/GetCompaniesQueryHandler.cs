using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Domain.BusinessManagement;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanies
{
    public class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, GetCompaniesQueryResponse>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserManagementClient _managementClient;


        public GetCompaniesQueryHandler(IBusinessDbContext context, IMapper mapper, IUserManagementClient managementClient, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _managementClient = managementClient;
        }

        public async Task<GetCompaniesQueryResponse> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var dtos = new List<CompanyDTO>();
            
            if (!string.IsNullOrEmpty(request.UserId))
            {
                var companies = _context.TeamMembers.AsNoTracking()
                    .Include(tm => tm.Company)
                    .Where(u => u.UserId == request.UserId)
                    .Select(tm => tm.Company);
                
                foreach (var company in companies)
                {
                    var dto = _mapper.Map<CompanyDTO>(company);
                    var userPermissions =
                        await _managementClient.GetUserPermissionsAsync(request.UserId, company.Id,
                            cancellationToken);
                    if (userPermissions.HasPermission(Authorization.Base.Constants.Permissions.Company.CanRead, company.Id))
                        dtos.Add(dto);
                }
                return new GetCompaniesQueryResponse
                {
                    Entities = dtos,
                    Total = dtos.Count
                };
            }
            else
            {
                var companiesQuery = _context.Companies.AsNoTracking();
                var count = await companiesQuery.CountAsync(cancellationToken);
                var items = await companiesQuery.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken);
                return new GetCompaniesQueryResponse
                {
                    Entities = items.Select(c => _mapper.Map<CompanyDTO>(c)),
                    Total = count
                };
            }
        }
    }
}