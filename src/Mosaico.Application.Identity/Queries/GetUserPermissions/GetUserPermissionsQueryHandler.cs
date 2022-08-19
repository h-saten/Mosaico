using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Serilog;

namespace Mosaico.Application.Identity.Queries.GetUserPermissions
{
    public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, GetUserPermissionsResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _readRepository;
        private readonly ILogger _logger;

        public GetUserPermissionsQueryHandler(IMapper mapper, IUserReadRepository readRepository, ILogger logger = null)
        {
            _mapper = mapper;
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<GetUserPermissionsResponse> Handle(GetUserPermissionsQuery request, CancellationToken cancellationToken)
        {
            List<UserToPermission> userPermissions;
            if (request.EntityId.HasValue)
            {
                userPermissions = await _readRepository.GetUserPermissionsAsync(request.Id, request.EntityId.Value, cancellationToken);
            }
            else
            {
                userPermissions = await _readRepository.GetUserPermissionsAsync(request.Id, cancellationToken);
            }

            return new GetUserPermissionsResponse
            {
                Permissions = userPermissions.Select(p => _mapper.Map<UserPermissionDTO>(p)).ToList()
            };
        }
    }
}