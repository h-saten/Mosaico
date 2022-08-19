using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.ValueObjects;
using Serilog;

namespace Mosaico.Application.Identity.Queries.FindUser
{
    public class FindUserQueryHandler : IRequestHandler<FindUserQuery, GetUserQueryResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        
        public FindUserQueryHandler(IMapper mapper, IUserReadRepository readRepository, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<GetUserQueryResponse> Handle(FindUserQuery request, CancellationToken cancellationToken)
        {
            ApplicationUser user = null;
            
            if (request.FindBy == Constants.UserFindFields.Email)
            {
                var userEmail = new EmailAddress(request.Value);
                user = await _readRepository.GetAsync(userEmail.Address, cancellationToken);
            }
            
            return user is null ? null : _mapper.Map<GetUserQueryResponse>(user);
        }
    }
}