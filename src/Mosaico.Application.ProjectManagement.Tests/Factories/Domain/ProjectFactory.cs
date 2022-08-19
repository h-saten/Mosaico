using System;
using FizzWare.NBuilder;
using Mosaico.Domain.ProjectManagement.Entities;

namespace Mosaico.Application.ProjectManagement.Tests.Factories.Domain
{
    internal static class ProjectFactory
    {
        public static Project Create()
        {
            var project = Builder<Project>.CreateNew().Build();
            project.Id = Guid.NewGuid();
            return project;
        }
        
        public static Project WithActiveStage()
        {
            var project = Create();
            project.TokenId = Guid.NewGuid();
            project.Stages.Add(StageFactory.Pending(project.Id));
            project.Stages.Add(StageFactory.Active(project.Id));
            return project;
        }
    }
}