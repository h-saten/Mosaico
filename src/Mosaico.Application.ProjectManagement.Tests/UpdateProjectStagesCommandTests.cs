using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.ProjectManagement.Commands.UpdateProjectStages;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.BackgroundJobs.Base;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.Tests.Base;
using NUnit.Framework;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.ProjectManagement.Tests
{
    [TestFixture]
    public class UpdateProjectStagesCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;

        protected override List<Profile> Profiles => new()
        {
            new ProjectManagementMapperProfile()
        };
        
        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            _eventFactory = new CloudEventFactory();
            builder.RegisterInstance(_eventFactory).As<IEventFactory>();
            RegisterContext<ProjectContext>(builder);
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).As<IEventPublisher>();
            var backgroundJobProvider = new Mock<IBackgroundJobProvider>();
            backgroundJobProvider.Setup(b => b.Add(It.IsAny<string>(), It.IsAny<DateTimeOffset>(), TimeZoneInfo.Utc,
                    "default", It.IsAny<Guid>()))
                .Returns(Guid.NewGuid().ToString());
            builder.RegisterInstance(backgroundJobProvider.Object).As<IBackgroundJobProvider>();
            builder.RegisterType<UpdateProjectStagesCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<UpdateProjectStagesCommandValidator>().As<IValidator<UpdateProjectStagesCommand>>();
        }
        
        private async Task<Project> InitializeProject(ProjectContext context)
        {
            var project = new Project
            {
                Description = "Description",
                Title = "Test Project",
                TokenId = Guid.NewGuid()
            };
            project.SetStatus(context.ProjectStatuses.FirstOrDefault());
            context.Add(project);
            await context.SaveChangesAsync();
            return project;
        }

        [Test]
        public async Task ShouldCreateStages()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Name = "Stage 1",
                        Type = StageType.Public,
                        MaximumPurchase = 1000,
                        MinimumPurchase = 100,
                        TokenPrice = 10,
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        TokensSupply = 10000
                    }
                }
            };
            // Act
            await SendAsync(command);
            // Assert
            var dbProject = await context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
            dbProject.Should().NotBeNull();
            dbProject.Stages.Should().NotBeNull();
            dbProject.Stages.Count.Should().Be(1);
            var stage = dbProject.Stages.FirstOrDefault();
            stage.Name.Should().Be("Stage 1");
            stage.Status.Should().NotBeNull();
            stage.Status.Key.Should().Be(Domain.ProjectManagement.Constants.StageStatuses.Pending);
        }
        
        [Test]
        public async Task ShouldFailIfStageHasInvalidName()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Type = StageType.Public,
                        MaximumPurchase = 1000,
                        MinimumPurchase = 100,
                        TokenPrice = 10,
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        TokensSupply = 10000
                    }
                }
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
        }
        
        [Test]
        public async Task ShouldFailIfStageHasInvalidMinimumPurchase()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Name = "Stage 1",
                        Type = StageType.Public,
                        MaximumPurchase = 1000,
                        MinimumPurchase = -1,
                        TokenPrice = 10,
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        TokensSupply = 10000
                    }
                }
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
        }
        
        [Test]
        public async Task ShouldFailIfStageHasInvalidMaximumPurchase()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Name = "Stage 1",
                        Type = StageType.Public,
                        MaximumPurchase = 10,
                        MinimumPurchase = 100,
                        TokenPrice = 10,
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        TokensSupply = 10000
                    }
                }
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
        }
        
        [Test]
        public async Task ShouldFailIfStageHasInvalidPrice()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Name = "Stage 1",
                        Type = StageType.Public,
                        MaximumPurchase = 1000,
                        MinimumPurchase = 100,
                        TokenPrice = 0,
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        TokensSupply = 10000
                    }
                }
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
        }
        
        [Test]
        public async Task ShouldFailIfStageHasInvalidStartDate()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Name = "Stage 1",
                        Type = StageType.Public,
                        MaximumPurchase = 1000,
                        MinimumPurchase = 100,
                        TokenPrice = 10,
                        StartDate = DateTimeOffset.MinValue,
                        TokensSupply = 10000
                    }
                }
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
        }
        
        [Test]
        public async Task ShouldFailIfStageHasInvalidTokenSupply()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Name = "Stage 1",
                        Type = StageType.Public,
                        MaximumPurchase = 1000,
                        MinimumPurchase = 100,
                        TokenPrice = 10,
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        TokensSupply = 0
                    }
                }
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
        }
        
        [Test]
        public async Task ShouldUpdateExistingStage()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitializeProject(context);
            project.Stages.Add(new Stage
            {
                Name = "Stage 1",
                Type = StageType.Public,
                MaximumPurchase = 1000,
                MinimumPurchase = 100,
                TokenPrice = 10,
                StartDate = DateTimeOffset.UtcNow.AddDays(6),
                TokensSupply = 10000
            });
            context.Update(project);
            await context.SaveChangesAsync();
            
            var command = new UpdateProjectStagesCommand
            {
                Id = project.Id,
                Stages = new List<StageCreationDTO>
                {
                    new StageCreationDTO
                    {
                        Name = "Stage 111",
                        Type = StageType.Public,
                        MaximumPurchase = 1000,
                        MinimumPurchase = 500,
                        TokenPrice = 1,
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        TokensSupply = 100000
                    }
                }
            };
            // Act
            await SendAsync(command);
            // Assert
            var dbProject = await context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
            dbProject.Should().NotBeNull();
            dbProject.Should().NotBeNull();
            dbProject.Stages.Should().NotBeNull();
            dbProject.Stages.Count.Should().Be(1);
            var stage = dbProject.Stages.FirstOrDefault();
            stage.Name.Should().Be("Stage 111");
            stage.TokenPrice.Should().Be(1);
            stage.TokensSupply.Should().Be(100000);
            stage.Status.Should().NotBeNull();
            stage.Status.Key.Should().Be(Domain.ProjectManagement.Constants.StageStatuses.Pending);
        }
    }
}