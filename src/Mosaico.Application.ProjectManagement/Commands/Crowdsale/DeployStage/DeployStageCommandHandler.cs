using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployStage
{
    public class DeployStageCommandHandler : IRequestHandler<DeployStageCommand>
    {
        private readonly IProjectDbContext _projectDbContext;
        private readonly IStageDeploymentJob _stageDeployment;
        private readonly IBackgroundJobProvider _backgroundJob;
        private readonly ICurrentUserContext _userContext;

        public DeployStageCommandHandler(IProjectDbContext projectDbContext, IStageDeploymentJob stageDeployment, IBackgroundJobProvider backgroundJob, ICurrentUserContext userContext)
        {
            _projectDbContext = projectDbContext;
            _stageDeployment = stageDeployment;
            _backgroundJob = backgroundJob;
            _userContext = userContext;
        }

        public async Task<Unit> Handle(DeployStageCommand request, CancellationToken cancellationToken)
        {
            var stage = await _projectDbContext.Stages
                .Include(s => s.Project).ThenInclude(p => p.Crowdsale)
                .Include(s => s.Status)
                .FirstOrDefaultAsync(s => s.ProjectId == request.ProjectId && s.Id == request.StageId, cancellationToken: cancellationToken);

            if (stage == null || stage.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Closed)
            {
                throw new StageNotFoundException(request.StageId);
            }

            if (stage.DeploymentStatus == StageDeploymentStatus.Deployed && !stage.AllowRedeployment)
            {
                throw new NotAllowedRedeploymentException(stage.Id.ToString());
            }
                
            var contractAddress = stage.Project.Crowdsale.ContractAddress;
            if (string.IsNullOrWhiteSpace(contractAddress))
            {
                throw new EmptyContractAddressException(stage.ProjectId.ToString());
            }
            _backgroundJob.Execute(() => _stageDeployment.DeployStageAsync(request.StageId, _userContext.UserId));
            return Unit.Value;
        }
    }
}