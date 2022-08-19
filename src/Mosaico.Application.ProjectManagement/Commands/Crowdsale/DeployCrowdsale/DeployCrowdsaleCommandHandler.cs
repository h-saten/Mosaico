using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Authorization.Base;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.Wallet.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployCrowdsale
{
    public class DeployCrowdsaleCommandHandler : IRequestHandler<DeployCrowdsaleCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IBackgroundJobProvider _backgroundJob;
        private readonly ICrowdsaleDeploymentJob _crowdsaleDeployment;
        private readonly ICurrentUserContext _currentUser;

        public DeployCrowdsaleCommandHandler(IProjectDbContext projectDbContext, IBackgroundJobProvider backgroundJob, ICrowdsaleDeploymentJob crowdsaleDeployment, ICurrentUserContext currentUser)
        {
            _projectDbContext = projectDbContext;
            _backgroundJob = backgroundJob;
            _crowdsaleDeployment = crowdsaleDeployment;
            _currentUser = currentUser;
        }

        public async Task<Unit> Handle(DeployCrowdsaleCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken: cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }

            if (!project.TokenId.HasValue)
            {
                throw new TokenNotFoundException($"project {project.Id}");
            }

            _backgroundJob.Execute(() => _crowdsaleDeployment.DeployCrowdsaleAsync(project.Id, _currentUser.UserId));
            return Unit.Value;
        }
    }
}