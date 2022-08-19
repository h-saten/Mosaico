using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Application.BusinessManagement.Queries.GetInvitation;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Entities;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetInvitation
{
    public class GetInvitationQueryHandler : IRequestHandler<GetInvitationQuery, GetInvitationQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IBusinessDbContext _businessDbContext;
        private readonly IMapper _mapper;

        public GetInvitationQueryHandler(IBusinessDbContext businessDbContext,  IMapper mapper, ILogger logger = null)
        {
            _businessDbContext = businessDbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetInvitationQueryResponse> Handle(GetInvitationQuery request, CancellationToken cancellationToken)
        {
            var invitation = await _businessDbContext.TeamMembers.FirstOrDefaultAsync(i => i.Id == request.Id && i.CompanyId == request.CompanyId, cancellationToken);
            var dto = _mapper.Map<CompanyInvitationDTO>(invitation);
            return new GetInvitationQueryResponse
            {
                Invitation = dto
            };
        }
    }
}