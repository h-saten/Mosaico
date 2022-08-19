using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertProjectStage
{
    public class UpsertProjectStageCommandHandler : IRequestHandler<UpsertProjectStageCommand, Guid>
    {
        private readonly IProjectDbContext _projectDbContext;

        public UpsertProjectStageCommandHandler(IProjectDbContext projectDbContext)
        {
            _projectDbContext = projectDbContext;
        }

        public async Task<Guid> Handle(UpsertProjectStageCommand request, CancellationToken cancellationToken)
        {
            var project = await _projectDbContext.Projects
                .Include(p => p.Stages)
                .FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);

            if (project == null)
            {
                throw new ProjectNotFoundException(request.ProjectId);
            }
            
            var defaultStageStatus = await _projectDbContext.StageStatuses.FirstOrDefaultAsync(
                s => s.Key == Domain.ProjectManagement.Constants.StageStatuses.Pending, cancellationToken);
            if (defaultStageStatus == null)
            {
                throw new StageStatusNotFoundException(Domain.ProjectManagement.Constants.StageStatuses.Pending);
            }

            Stage stage = default;
            if (request.StageId.HasValue && request.StageId.Value != Guid.Empty)
            {
                stage = project.Stages.FirstOrDefault(s => s.Id == request.StageId.Value);
                if (stage == null)
                {
                    throw new StageNotFoundException(request.ProjectId, request.StageId.Value);
                }
                
                stage.EndDate = request.EndDate;
                stage.Type = request.Type;
                stage.MinimumPurchase = request.MinimumPurchase;
                stage.StartDate = request.StartDate;
                stage.TokenPrice = request.TokenPrice;
                stage.TokensSupply = request.TokenSupply;
                stage.Name = request.Name;
                
            }
            else
            {
                if (project.Stages.Any(s =>
                    string.Equals(s.Name, request.Name, StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new StageAlreadyExistsException(request.Name);
                }

                stage = new Stage
                {
                    Name = request.Name,
                    Status = defaultStageStatus,
                    Type = request.Type,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    MaximumPurchase = request.MaximumPurchase,
                    MinimumPurchase = request.MinimumPurchase,
                    TokenPrice = request.TokenPrice,
                    TokensSupply = request.TokenSupply
                };
                project.Stages.Add(stage);
            }

            if (stage.Type == 0)
            {
                stage.Type = StageType.Public;
            }
            
            if (stage.Type != StageType.Public && string.IsNullOrWhiteSpace(stage.AuthorizationCode))
            {
                stage.AuthorizationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
            }

            project.Stages = project.Stages.OrderBy(s => s.StartDate).ToList();
            CheckOverlappingStages(project.Stages);
            await _projectDbContext.SaveChangesAsync(cancellationToken);
            return project.Stages.FirstOrDefault(s => s.Name == request.Name).Id;
        }

        private void CheckOverlappingStages(List<Stage> stages)
        {
            var timestamps = stages.Select((s, index) => 
                new Tuple<int, long, long>(index, s.StartDate.ToUnixTimeSeconds(), s.EndDate.Value.ToUnixTimeSeconds()))
                .ToList();
            foreach (var timestamp in timestamps)
            {
                if (timestamps.Where(t => timestamp.Item1 != t.Item1).Any(t => timestamp.Item2 < t.Item3 && t.Item2 < timestamp.Item3))
                {
                    throw new StageOverlappingException();
                }
            }
        }
        
    }
}