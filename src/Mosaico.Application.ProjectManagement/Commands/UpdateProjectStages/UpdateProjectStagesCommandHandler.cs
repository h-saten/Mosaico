using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Extensions;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Cache.Base.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Domain.ProjectManagement.Extensions;
using Serilog;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectStages
{
    public class UpdateProjectStagesCommandHandler : IRequestHandler<UpdateProjectStagesCommand, List<StageDTO>>
    {
        private readonly IProjectDbContext _projectDb;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly ICacheClient _cacheClient;

        public UpdateProjectStagesCommandHandler(IProjectDbContext projectDb, IMapper mapper, ICacheClient cacheClient, ILogger logger = null)
        {
            _projectDb = projectDb;
            _mapper = mapper;
            _cacheClient = cacheClient;
            _logger = logger;
        }

        public async Task<List<StageDTO>> Handle(UpdateProjectStagesCommand request, CancellationToken cancellationToken)
        {
            using (var transaction = _projectDb.BeginTransaction())
            {
                try
                {
                    _logger?.Verbose($"Trying to update stages");
                    
                    var project = await _projectDb.GetProjectOrThrowAsync(request.Id.ToString(), cancellationToken);
                    
                    var defaultStageStatus = await _projectDb.StageStatuses.FirstOrDefaultAsync(
                        s => s.Key == Domain.ProjectManagement.Constants.StageStatuses.Pending, cancellationToken);
                    if (defaultStageStatus == null)
                    {
                        throw new StageStatusNotFoundException(Domain.ProjectManagement.Constants.StageStatuses.Pending);
                    }
                    var newStageIds = request.Stages.Where(s => s.Id.HasValue).Select(s => s.Id).ToList();
                    if (newStageIds.Any())
                    {
                        project.Stages.RemoveAll(s =>
                            !newStageIds.Contains(s.Id) &&
                            s.Status.Key == Domain.ProjectManagement.Constants.StageStatuses.Pending);
                    }
                    var lastOrderId = 1;
                    foreach (var stageDTO in request.Stages.OrderBy(s => s.StartDate))
                    {
                        var existingStage = project.Stages.FirstOrDefault(s => s.Id == stageDTO.Id);
                        if (existingStage != null)
                        {
                            _logger?.Verbose($"Updating existing {stageDTO.Name} stage");
                            existingStage.Update(stageDTO);
                            existingStage.Order = lastOrderId++;
                        }
                        else
                        {
                            _logger?.Verbose($"Creating new stage {stageDTO.Name}");
                            existingStage = _mapper.Map<Stage>(stageDTO);
                            if (stageDTO.Id.HasValue && stageDTO.Id.Value != Guid.Empty)
                            {
                                existingStage.Id = stageDTO.Id.Value;
                                _projectDb.Entry(existingStage).State = EntityState.Added;
                            }
                            existingStage.SetProject(project);
                            existingStage.SetStatus(defaultStageStatus);
                            existingStage.Order = lastOrderId++;
                            project.Stages.Add(existingStage);
                        }

                        if (existingStage.Type == 0)
                        {
                            existingStage.Type = StageType.Public;
                        }
                        
                        if (existingStage.Type != StageType.Public && string.IsNullOrWhiteSpace(existingStage.AuthorizationCode))
                        {
                            existingStage.AuthorizationCode = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
                        }
                    }
                    
                    CheckOverlappingStages(project.Stages);
                    project.Crowdsale ??= new Domain.ProjectManagement.Entities.Crowdsale();
                    project.Crowdsale.HardCap = project.Stages.Sum(s => s.TokensSupply);
                    //TODO: make it configurable using settings
                    project.Crowdsale.SoftCap = Math.Ceiling(project.Crowdsale.HardCap * 0.3m);
                    
                    _projectDb.Projects.Update(project);
                    await _projectDb.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    _logger?.Verbose($"successfully saved new stages");
                    await _cacheClient.CleanAsync(new List<string>
                    {
                        $"{nameof(GetProjectQuery)}_{project.Slug}",
                        $"{nameof(GetProjectQuery)}_{project.Id}"
                    }, cancellationToken);
                    return project.Stages.Select(s => _mapper.Map<StageDTO>(s)).ToList();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            }
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