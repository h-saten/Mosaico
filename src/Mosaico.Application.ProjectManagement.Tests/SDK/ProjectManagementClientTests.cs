using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Mosaico.Application.ProjectManagement.Tests.Factories.Domain;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.SDK.ProjectManagement;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.ProjectManagement.Tests.SDK
{
    public class ProjectManagementClientTests : EFInMemoryTestBase
    {
        private IProjectDbContext _projectDbContext;

        protected override List<Profile> Profiles => new()
        {
            new ProjectManagementClientMapperProfile()
        };

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            _projectDbContext = RegisterContext<ProjectContext>(builder);
        }

        [Test]
        public async Task ShouldReturnProjectWithActiveStage()
        {
            var project = ProjectFactory.WithActiveStage();
            await _projectDbContext.Projects.AddAsync(project);
            await _projectDbContext.SaveChangesAsync();
            
            var sut = new ProjectManagementClient(_projectDbContext, Mapper());

            var result = await sut.GetProjectWithActiveSale((Guid) project.TokenId);
            
            Assert.NotNull(result);
            Assert.NotNull(result.ActiveStageId);
        }

        [Test]
        public async Task ShouldReturnNullWhenProjectStagesNotExist()
        {
            var project = ProjectFactory.Create();
            await _projectDbContext.Projects.AddAsync(project);
            await _projectDbContext.SaveChangesAsync();
            
            var sut = new ProjectManagementClient(_projectDbContext, Mapper());

            var result = await sut.GetProjectWithActiveSale((Guid) project.TokenId);
            
            Assert.Null(result);
        }
    }
}