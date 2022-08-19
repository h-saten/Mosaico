using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Domain.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetUsersById
{
    public class GetUsersByIdQueryHandler : IRequestHandler<GetUsersByIdQuery, GetUsersByIdResponse>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        
        public GetUsersByIdQueryHandler(IMapper mapper, IUserReadRepository readRepository, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<GetUsersByIdResponse> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
        {
            var usersList = await _readRepository.GetUsersAsync(request.UsersId, cancellationToken);
            var users = usersList.Select(x => _mapper.Map<GetUserQueryResponse>(x)).ToList();
            return new GetUsersByIdResponse
            {
                Users = users
            };
        }
    }
}