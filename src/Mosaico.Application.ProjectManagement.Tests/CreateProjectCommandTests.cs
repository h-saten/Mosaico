using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.ProjectManagement.Commands.CreateProject;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Exceptions;
using Mosaico.Application.ProjectManagement.Validators;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.ProjectContext;
using Mosaico.SDK.BusinessManagement.Abstractions;
using Mosaico.SDK.BusinessManagement.Models;
using Mosaico.SDK.Identity.Abstractions;
using Mosaico.SDK.Wallet.Abstractions;
using Mosaico.SDK.Wallet.Models;
using Mosaico.Tests.Base;
using NUnit.Framework;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.ProjectManagement.Tests
{
    [TestFixture]
    public class CreateProjectCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;
        private Mock<IUserManagementClient> _managementClient;
        private Mock<IBusinessManagementClient> _businessClient;

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
            builder.RegisterInstance(walletClientMock.Object).As<IWalletClient>();

            _businessClient = new Mock<IBusinessManagementClient>();
            _businessClient.Setup(c => c.GetCompanyAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(new MosaicoCompany());
            builder.RegisterInstance(_businessClient.Object).As<IBusinessManagementClient>();

            builder.RegisterInstance(new StageValidator()).As<IValidator<StageCreationDTO>>();

            builder.RegisterType<CreateProjectCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<CreateProjectCommandValidator>().As<IValidator<CreateProjectCommand>>();
        }
        
        [Test]
        public async Task ShouldCreateProject()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var companyId = Guid.NewGuid();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                ShortDescription = "Test Description"
            };
            
            // Act
            var response = await SendAsync(command);
            
            // Assert
            response.Should().NotBe(Guid.Empty);
            
            var project = await context.Projects.FirstOrDefaultAsync(u => u.Id == response);
           
            project.Should().NotBeNull();
            project.Title.Should().Be("Test Project");
            project.Description.Should().Be("Test Description");
            project.Stages.Should().BeEmpty();
            project.CreatedById.Should().Be(CurrentUserContext.UserId);
            project.ModifiedById.Should().Be(CurrentUserContext.UserId);
            project.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
            project.ModifiedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
            project.Status.Should().NotBeNull();
            project.Status.Key.Should().Be(Domain.ProjectManagement.Constants.ProjectStatuses.New);
            project.CompanyId.Should().Be(companyId);
        
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }
        
        [Test]
        public async Task ShouldNotCreateProjectWithoutName()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var command = new CreateProjectCommand
            {
                ShortDescription = "Test Description"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
        
        [Test]
        public async Task ShouldNotCreateProjectWithoutTokenName()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                ShortDescription = "Test Description"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
        
        [Test]
        public async Task ShouldNotCreateProjectWithoutTokenSymbol()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                ShortDescription = "Test Description"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
        
        [Test]
        public async Task ShouldNotCreateProjectWithoutTokenSupply()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                ShortDescription = "Test Description"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotCreateProjectWithoutCompanyId()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var command = new CreateProjectCommand
            {
                ShortDescription = "Test Description"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotCreateProjectWithFakeNetwork()
        {
            // Arrange
            var context = GetContext<ProjectContext>();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                ShortDescription = "Test Description"
            };
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<ValidationException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotCreateProjectIfCompanyDoesNotExist()
        {
            // Arrange
            _businessClient.Setup(c => c.GetCompanyAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(null as MosaicoCompany);
            var context = GetContext<ProjectContext>();
            var command = new CreateProjectCommand
            {
                Title = "Test Project",
                ShortDescription = "Test Description"
            };
            
            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<CompanyNotExistsException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);
            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
    }
}