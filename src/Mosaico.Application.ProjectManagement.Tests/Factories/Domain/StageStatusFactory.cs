using System;
using FizzWare.NBuilder;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Tests.Factories.Domain
{
    internal static class StageStatusFactory
    {
        public static StageStatus Active()
        {
            var stageStatus = Builder<StageStatus>.CreateNew().Build();
            stageStatus.Id = Guid.NewGuid();
            stageStatus.Key = Mosaico.Domain.ProjectManagement.Constants.StageStatuses.Active;
            return stageStatus;
        }
        public static StageStatus Closed()
        {
            var stageStatus = Builder<StageStatus>.CreateNew().Build();
            stageStatus.Id = Guid.NewGuid();
            stageStatus.Key = Mosaico.Domain.ProjectManagement.Constants.StageStatuses.Closed;
            return stageStatus;
        }
        public static StageStatus Pending()
        {
            var stageStatus = Builder<StageStatus>.CreateNew().Build();
            stageStatus.Id = Guid.NewGuid();
            stageStatus.Key = Mosaico.Domain.ProjectManagement.Constants.StageStatuses.Pending;
            return stageStatus;
        }
    }
}