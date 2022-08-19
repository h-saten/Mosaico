using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Features.DTOs;
using Mosaico.Domain.Features.Abstractions;
using Serilog;

namespace Mosaico.Application.Features.Queries.GetAllEntitySettingsQuery
{
    public class GetAllEntitySettingsQueryHandler : IRequestHandler<GetAllEntitySettingsQuery, GetAllEntitySettingsQueryResponse>
    {
        private readonly IFeaturesDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        
        public GetAllEntitySettingsQueryHandler(IFeaturesDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetAllEntitySettingsQueryResponse> Handle(GetAllEntitySettingsQuery request, CancellationToken cancellationToken)
        {
            var settingsQuery = request.EntityId.HasValue ? 
                _context.Features.Where(x => x.Category == request.Category && x.EntityId == request.EntityId).AsNoTracking() :
                _context.Features.Where(x => x.Category == request.Category).AsNoTracking();
            
            var count = await settingsQuery.CountAsync(cancellationToken);
            var settings = await settingsQuery.ToListAsync(cancellationToken);
            var dtos = settings.Select(x => _mapper.Map<FeatureDTO>(x));

            return new GetAllEntitySettingsQueryResponse() { Entities = dtos, Total = count};
        }
    }
}