using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.DeleteStage
{
    public class DeleteStageCommandHandler : IRequestHandler<DeleteStageCommand>
    {
        private readonly IProjectDbContext _projectDbContext;

        public DeleteStageCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Unit> Handle(DeleteStageCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects.Include(p => p.Stages).FirstOrDefaultAsync(t => t.Id == request.ProjectId, cancellationToken);
            if (project == null)
            {
                throw new ProjectNotFoundException(request.StageId);
            }
            
            var stage = project.Stages.FirstOrDefault(s => s.Id == request.StageId);
            if (stage == null)
            {
                throw new StageNotFoundException(request.StageId);
            }

            project.Stages.Remove(stage);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}