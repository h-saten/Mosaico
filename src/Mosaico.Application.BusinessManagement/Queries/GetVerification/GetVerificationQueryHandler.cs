using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.BusinessManagement.Abstractions;
using Mosaico.Domain.BusinessManagement.Extensions;
using Mosaico.Domain.BusinessManagement;
using Serilog;
using Constants = Mosaico.Domain.BusinessManagement.Constants;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Mosaico.Domain.BusinessManagement.Exceptions;
using Mosaico.SDK.Identity.Abstractions;

namespace Mosaico.Application.BusinessManagement.Queries.GetVerification
{
    public class GetVerificationQueryHandler : IRequestHandler<GetVerificationQuery, GetVerificationQueryResponse>
    {
        private readonly IBusinessDbContext _context;
        private readonly IMapper _mapper;


        public GetVerificationQueryHandler(IBusinessDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<GetVerificationQueryResponse> Handle(GetVerificationQuery request, CancellationToken cancellationToken)
        {
            var verification = await _context.Verifications
                .Include(x=>x.Shareholders)
                .FirstOrDefaultAsync(x=> x.CompanyId == request.CompanyId, cancellationToken);

            if (verification == null)
            {
                return new GetVerificationQueryResponse();
            }
            
            var dto = _mapper.Map<VerificationDTO>(verification);
            dto.Shareholders = new List<ShareholderDTO>();
            foreach (var shareholder in verification.Shareholders.Select(s => _mapper.Map<ShareholderDTO>(s)))
            {
                dto.Shareholders.Add(shareholder);
            }
            return new GetVerificationQueryResponse
            {
                Verification = dto
            };
        }
    }
}