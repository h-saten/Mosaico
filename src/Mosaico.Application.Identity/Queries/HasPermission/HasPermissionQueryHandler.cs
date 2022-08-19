using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Mosaico.Domain.Identity.Abstractions;
using Serilog;

namespace Mosaico.Application.Identity.Queries.HasPermission
{
    public class HasPermissionQueryHandler : IRequestHandler<HasPermissionQuery, bool>
    {
        private readonly IUserReadRepository _readRepository;
        private readonly ILogger _logger;

        public HasPermissionQueryHandler(IUserReadRepository readRepository, ILogger logger = null)
        {
            _readRepository = readRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(HasPermissionQuery request, CancellationToken cancellationToken)
        {
            var userPermission = await _readRepository.GetUserPermission(request.UserId, request.Key, cancellationToken);
            return userPermission != null && ((!request.EntityId.HasValue && !userPermission.EntityId.HasValue) ||
                                              request.EntityId == userPermission.EntityId);
        }
    }
}