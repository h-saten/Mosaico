using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetKangaUser
{
    public class GetKangaUserQueryHandler : IRequestHandler<GetKangaUserQuery, GetKangaUserQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        private readonly IIdentityContext _identityContext;
        
        public GetKangaUserQueryHandler(IMapper mapper, IUserReadRepository readRepository, IIdentityContext identityContext, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _identityContext = identityContext;
            _logger = logger;
        }

        public async Task<GetKangaUserQueryResponse> Handle(GetKangaUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _readRepository.GetAsync(request.Id, cancellationToken);
            if (user == null) 
                throw new UserNotFoundException(request.Id);

            var kangaUser = await _identityContext
                .KangaUsers
                .Where(x => x.ApplicationUserId == user.Id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if (kangaUser is null)
                throw new KangaUserNotFoundException(request.Id);
            
            return new GetKangaUserQueryResponse
            {
                Email = user.Email,
                Id = user.Id,
                KangaKycVerified = kangaUser.KycVerified,
                KangaUserId = kangaUser.KangaAccountId
            };
        }
    }
}