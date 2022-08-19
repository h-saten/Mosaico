using System.Threading;
using System.Threading.Tasks;
using MassTransit.Initializers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;

namespace Mosaico.Application.ProjectManagement.Queries.GetStageAuthorizationCode
{
    public class GetStageAuthorizationCodeQueryHandler : IRequestHandler<GetStageAuthorizationCodeQuery, string>
    {
        private readonly IProjectDbContext _projectDbContext;

        public GetStageAuthorizationCodeQueryHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<string> Handle(GetStageAuthorizationCodeQuery request, CancellationToken cancellationToken)
        {
            var authorizationCode =
                await _projectDbContext.Stages.AsNoTracking().FirstOrDefaultAsync(
                    s => s.Id == request.StageId && s.ProjectId == request.ProjectId, cancellationToken)
                    .Select(s => s.AuthorizationCode);
            return authorizationCode;
        }
    }
}