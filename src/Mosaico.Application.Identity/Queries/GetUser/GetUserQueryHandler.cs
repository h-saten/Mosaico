using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Exceptions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        private readonly IIdentityContext _identityContext;
        
        public GetUserQueryHandler(IMapper mapper, IUserReadRepository readRepository, IIdentityContext identityContext, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _identityContext = identityContext;
            _logger = logger;
        }

        public async Task<GetUserQueryResponse> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _readRepository.GetAsync(request.Id, cancellationToken);
            if (user == null) 
                throw new UserNotFoundException(request.Id);

            var response = _mapper.Map<GetUserQueryResponse>(user);
            
            var kangaUserAccountExist = await _identityContext.KangaUsers.AsNoTracking()
                .AnyAsync(x => x.ApplicationUserId == user.Id, cancellationToken);
            response.HasKangaAccount = kangaUserAccountExist;
            
            return response;
        }
    }
}