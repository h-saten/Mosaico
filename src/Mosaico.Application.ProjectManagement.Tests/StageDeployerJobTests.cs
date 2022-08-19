using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.ProjectManagement.BackgroundJobs;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Integration.Blockchain.Ethereum.Abstractions;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.Tests.Base;
using NUnit.Framework;

namespace Mosaico.Application.ProjectManagement.Tests
{
    // [TestFixture]
    // public class StageDeployerJobTests : EFInMemoryTestBase
    // {
    //     private Mock<IEventPublisher> _eventPublisherMock;
    //     private IEventFactory _eventFactory;
    //     private Mock<IBackgroundJobProvider> _backgroundJobProvider;
    //
    //     protected override List<Profile> Profiles => new List<Profile>
    //     {
    //         new ProjectManagementMapperProfile()
    //     };
    //     protected override void RegisterDependencies(ContainerBuilder builder)
    //     {
    //         base.RegisterDependencies(builder);
    //         _eventFactory = new CloudEventFactory();
    //         builder.RegisterInstance(_eventFactory).As<IEventFactory>();
    //         RegisterContext<ProjectContext>(builder);
    //         _eventPublisherMock = new Mock<IEventPublisher>();
    //         _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
    //         builder.RegisterInstance(_eventPublisherMock.Object).As<IEventPublisher>();
    //     }
    //
    //     public StageDeployerJob InitializeJobAsync(ProjectContext context)
    //     {
    //         _backgroundJobProvider = new Mock<IBackgroundJobProvider>();
    //         _backgroundJobProvider
    //             .Setup(s => s.Add(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<TimeZoneInfo>(), "default", It.IsAny<object>()))
    //             .Verifiable();
    //
    //         var crowdsaleMock = new Mock<ICrowdsaleService>();
    //         crowdsaleMock.Setup(c =>
    //             c.StartNewStageAsync(It.IsAny<string>(), It.IsAny<Action<ContractStageConfiguration>>()));
    //         
    //         return new StageDeployerJob(context, _backgroundJobProvider.Object, _eventPublisherMock.Object, _eventFactory, crowdsaleMock.Object, null);
    //     }
    //
    //     [Test]
    //     public async Task ShouldDeployNewStage()
    //     {
    //         // Arrange
    //         var context = GetContext<ProjectContext>();
    //         var status = await context.ProjectStatuses.FirstOrDefaultAsync(s =>
    //             s.Key == Domain.ProjectManagement.Constants.ProjectStatuses.Approved);
    //         var stageStatus = await context.StageStatuses.FirstOrDefaultAsync(s =>
    //             s.Key == Domain.ProjectManagement.Constants.StageStatuses.Pending);
    //         var project = new Project
    //         {
    //             Description = "Description",
    //             Network = Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
    //             Title = "Test Project",
    //             TokenId = Guid.NewGuid(),
    //             IsVestingEnabled = false,
    //             IsStakingEnabled = false,
    //             Crowdsale = new Crowdsale
    //             {
    //                 ContractAddress = Guid.NewGuid().ToString()
    //             }
    //         };
    //         project.SetStatus(status);
    //         
    //         var stage = new Stage
    //         {
    //             Name = "Stage 1",
    //             IsPrivate = false,
    //             MaximumPurchase = 1000,
    //             MinimumPurchase = 100,
    //             TokenPrice = 10,
    //             StartDate = DateTimeOffset.UtcNow.AddDays(6),
    //             TokensSupply = 10000,
    //             EndDate = DateTimeOffset.UtcNow.AddDays(12)
    //         };
    //         stage.SetStatus(stageStatus);
    //         project.Stages.Add(stage);
    //         
    //         context.Projects.Add(project);
    //         await context.SaveChangesAsync();
    //         
    //         //Act
    //         var job = InitializeJobAsync(context);
    //         await job.ExecuteAsync(stage.Id.ToString());
    //         
    //         //Assert
    //         var dbStage = await context.Stages.FirstOrDefaultAsync(s => s.Id == stage.Id);
    //         dbStage.Status.Key.Should().Be(Domain.ProjectManagement.Constants.StageStatuses.Active);
    //         dbStage.StageJobs.Count.Should().Be(1);
    //         dbStage.StageJobs.FirstOrDefault().JobName.Should().Be(Constants.Jobs.StageFinalizationJob);
    //     }
        
    //}
}