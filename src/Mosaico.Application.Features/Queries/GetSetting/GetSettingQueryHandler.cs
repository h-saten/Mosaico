using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.Features.DTOs;
using Mosaico.Domain.Features;
using Mosaico.Domain.Features.Abstractions;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Identity.Extensions;
using Org.BouncyCastle.Security;
using Serilog;

namespace Mosaico.Application.Features.Queries.GetSetting
{
    public class GetSettingQueryHandler : IRequestHandler<GetSettingQuery, GetSettingQueryResponse>
    {
        private readonly IFeaturesDbContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;


        public GetSettingQueryHandler(IFeaturesDbContext context, IMapper mapper, ILogger logger = null)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSettingQueryResponse> Handle(GetSettingQuery request, CancellationToken cancellationToken)
        {
            var setting = request.EntityId.HasValue ?
                await _context.Features.FirstOrDefaultAsync(x => x.Category == request.Category && x.EntityId == request.EntityId && x.FeatureName == request.Name,cancellationToken) :
                await _context.Features.FirstOrDefaultAsync(x => x.Category == request.Category && x.FeatureName == request.Name, cancellationToken);
            
            return  _mapper.Map<GetSettingQueryResponse>(setting);
        }
    }
}