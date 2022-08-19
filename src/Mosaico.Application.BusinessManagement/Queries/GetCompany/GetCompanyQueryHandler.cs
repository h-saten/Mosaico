using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Extensions;
using Serilog;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompany
{
    public class GetCompanyQueryHandler : IRequestHandler<GetCompanyQuery, GetCompanyQueryResponse>
    {
        private readonly IBusinessDbContext _context;
        private readonly ILogger _logger;
        private readonly ICurrentUserContext _currentUser;
        private readonly IMapper _mapper;

        public GetCompanyQueryHandler(IBusinessDbContext context, IMapper mapper, ICurrentUserContext currentUser, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _currentUser = currentUser;
            _logger = logger;
        }

        public async Task<GetCompanyQueryResponse> Handle(GetCompanyQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.GetCompanyOrThrowAsync(request.UniqueIdentifier, cancellationToken);
            var dto = _mapper.Map<CompanyDTO>(company);
            var isSubscribed = false;
            if (_currentUser.IsAuthenticated)
            {
                isSubscribed = await _context.CompanySubscribers.AnyAsync(c => c.CompanyId == company.Id && c.UserId == _currentUser.UserId, cancellationToken);
            }
            
            return new GetCompanyQueryResponse
            {
                Company = dto,
                IsSubscribed = isSubscribed
            };
        }
    }
}