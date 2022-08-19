using System;
using FizzWare.NBuilder;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Tests.Factories.Domain
{
    internal static class StageFactory
    {
        public static Stage Pending(Guid projectId)
        {
            var stage = Builder<Stage>.CreateNew().Build();
            stage.Id = Guid.NewGuid();
            stage.ProjectId = projectId;
            stage.SetStatus(StageStatusFactory.Pending());
            return stage;
        }
        
        public static Stage Active(Guid projectId)
        {
            var stage = Pending(projectId);
            stage.SetStatus(StageStatusFactory.Active());
            return stage;
        }
    }
}