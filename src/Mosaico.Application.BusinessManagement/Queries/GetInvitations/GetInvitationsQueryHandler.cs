using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Application.BusinessManagement.Queries.GetInvitations;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetInvitations
{
    public class GetInvitationsQueryHandler : IRequestHandler<GetInvitationsQuery, GetCompanyInvitationsQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IBusinessDbContext _businessDbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserContext _currentUserContext;


        public GetInvitationsQueryHandler(IBusinessDbContext businessDbContext,  IMapper mapper, ICurrentUserContext currentUserContext, ILogger logger = null)
        {
            _currentUserContext = currentUserContext;
            _businessDbContext = businessDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCompanyInvitationsQueryResponse> Handle(GetInvitationsQuery request, CancellationToken cancellationToken)
        {

            var companyInvitationsQuery = _businessDbContext.TeamMembers
                .Where(p => p.CompanyId == request.CompanyId)
                .AsQueryable()
                .AsNoTracking();
            
            var companyInvitations = await companyInvitationsQuery.Include(s=>s.TeamMemberRole).ToListAsync(cancellationToken: cancellationToken);
            var totalCount = await companyInvitationsQuery.CountAsync(cancellationToken: cancellationToken);
            var dtos = new List<CompanyInvitationDTO>();

            foreach (var inv in companyInvitations)
            {
                var dto = _mapper.Map<CompanyInvitationDTO>(inv);
                dto.Expired = inv.ExpiresAt.HasValue && DateTimeOffset.UtcNow > inv.ExpiresAt.Value && inv.CreatedById != inv.UserId;
                dto.RoleName = inv.TeamMemberRole.Key;
                dto.CanEditRole = inv.UserId != _currentUserContext.UserId;
                dtos.Add(dto);
            }

            return new GetCompanyInvitationsQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }
    }
}