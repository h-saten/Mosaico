using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.ProjectManagement.Commands.UpdateProject;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Validators;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;
using Mosaico.SDK.Wallet.Models;
using Mosaico.Tests.Base;
using NUnit.Framework;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.ProjectManagement.Tests
{
    [TestFixture]
    public class UpdateProjectCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;
        private Mock<IUserManagementClient> _managementClient;

        protected override List<Profile> Profiles => new List<Profile>
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
            _managementClient = new Mock<IUserManagementClient>();
            builder.RegisterInstance(_managementClient.Object).As<IUserManagementClient>();
            var walletClientMock = new Mock<IWalletClient>();
            walletClientMock.Setup(c => c.CreateTokenAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<long>(),
                    It.IsAny<string>(), false, false, It.IsAny<string>()))
                .ReturnsAsync(new MosaicoToken
                {
                    Id = Guid.NewGuid(),
                    Name = "Test TOKEN",
                    Network = Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
                    Symbol = "TTOK",
                    TotalSupply = 100000
                });
            walletClientMock.Setup(c => c.GetTokenAsync(It.IsAny<Guid>())).ReturnsAsync(new MosaicoToken
            {
                Id = Guid.NewGuid(),
                Name = "Test TOKEN",
                Network = Mosaico.Blockchain.Base.Constants.BlockchainNetworks.Ethereum,
                Symbol = "TTOK",
                TotalSupply = 100000
            });
            builder.RegisterInstance(walletClientMock.Object).As<IWalletClient>();
            builder.RegisterInstance(new StageValidator()).As<IValidator<StageCreationDTO>>();

            builder.RegisterType<UpdateProjectCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<UpdateProjectCommandValidator>().As<IValidator<UpdateProjectCommand>>();
        }
        
        private async Task<Project> InitiateProjectStatuses(ProjectContext context)
        {
            var project = new Project
            {
                Description = "Description",
                Title = "Test Project",
                TokenId = Guid.NewGuid(),
                Stages = new List<Stage>
                {
                    new Stage
                    {
                        Name = "Test Stage",
                        StartDate = DateTimeOffset.UtcNow.AddDays(6),
                        EndDate = DateTimeOffset.UtcNow.AddDays(6).AddDays(3),
                        MinimumPurchase = 100,
                        MaximumPurchase = 1000,
                        TokenPrice = (decimal) 0.25,
                        TokensSupply = 10000
                    }
                }
            };
            project.SetStatus(context.ProjectStatuses.FirstOrDefault());
            context.Add(project);
            await context.SaveChangesAsync();
            return project;
        }

        [Test]
        public async Task ShouldUpdateProject()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitiateProjectStatuses(context);
            var command = new UpdateProjectCommand
            {
                
                ProjectId = project.Id
            };
            
            // Act
            var response = await SendAsync(command);
            
            // Assert
            var dbProject = await context.Projects.FirstOrDefaultAsync(p => p.Id == project.Id);
            dbProject.Should().NotBeNull();
            dbProject.Description.Should().Be("Description 2");
            dbProject.Title.Should().Be("A");
        }
        
        [Test]
        public async Task ShouldFailIfProjectTitleIsEmpty()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitiateProjectStatuses(context);
            var command = new UpdateProjectCommand
            {
                ProjectId = project.Id
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
        public async Task ShouldFailIfProjectIdEmpty()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitiateProjectStatuses(context);
            var command = new UpdateProjectCommand
            {
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ProjectNotFoundException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);
        }
        
        [Test]
        public async Task ShouldFailIfProjectNotFound()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var project = await InitiateProjectStatuses(context);
            var command = new UpdateProjectCommand
            {
                ProjectId = Guid.NewGuid()
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            // Assert
            await action.Should().ThrowAsync<ProjectNotFoundException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);
        }
    }
}