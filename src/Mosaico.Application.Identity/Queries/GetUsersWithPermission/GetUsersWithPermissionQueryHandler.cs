using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.Identity.Queries.GetUser;
using Mosaico.Authorization.Base;
using Mosaico.Base;
using Mosaico.Domain.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetUsersWithPermission
{
    public class GetUsersWithPermissionQueryHandler : IRequestHandler<GetUsersWithPermissionQuery, GetUsersWithPermissionResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        private readonly ICurrentUserContext _currentUserContext;
        private readonly ILogger _logger;

        public GetUsersWithPermissionQueryHandler(IMapper mapper, IUserReadRepository readRepository, ICurrentUserContext currentUserContext, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _currentUserContext = currentUserContext;
            _logger = logger;
        }

        public async Task<GetUsersWithPermissionResponse> Handle(GetUsersWithPermissionQuery request, CancellationToken cancellationToken)
        {
            var  usersWithPermissions = await _readRepository.GetUsersWithPermission(request.Key, cancellationToken);
            var users = usersWithPermissions.Entities.Select(x => _mapper.Map<GetUserQueryResponse>(x)).ToList();
            var result = new PaginatedResult<GetUserQueryResponse>() { Entities = users, Total = usersWithPermissions.Total };
            return new GetUsersWithPermissionResponse
            {
                Users = result
            };
        }
    }
}