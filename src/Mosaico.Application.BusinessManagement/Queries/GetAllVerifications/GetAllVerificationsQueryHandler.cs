using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetAllVerifications
{
    public class GetAllVerificationsQueryHandler : IRequestHandler<GetAllVerificationsQuery, GetAllVerificationsQueryResponse>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public GetAllVerificationsQueryHandler(IBusinessDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllVerificationsQueryResponse> Handle(GetAllVerificationsQuery request, CancellationToken cancellationToken)
        {
            var verificationsQuery = _context.Verifications.AsQueryable().AsNoTracking();
            var companiesQuery = _context.Companies.AsQueryable().AsNoTracking();
            var verifications = await verificationsQuery.Skip(request.Skip).Take(request.Take).ToListAsync(cancellationToken: cancellationToken);
            var totalCount = await _context.Verifications.CountAsync(cancellationToken: cancellationToken);
            var dtos = new List<VerificationDTO>();
            var companiesIds = verifications.Select(x => x.CompanyId).ToList();
            var companiesWithVerifications = await companiesQuery.Where(x => companiesIds.Contains(x.Id)).ToListAsync(cancellationToken: cancellationToken);
            foreach (var v in verifications)
            {
                var dto = _mapper.Map<VerificationDTO>(v);
                dto.CompanyName = companiesWithVerifications.FirstOrDefault(x => x.Id == v.CompanyId)?.CompanyName;
                dtos.Add(dto);
            }
            
            return new GetAllVerificationsQueryResponse
            {
                Entities = dtos,
                Total = totalCount
            };
        }
    }
}