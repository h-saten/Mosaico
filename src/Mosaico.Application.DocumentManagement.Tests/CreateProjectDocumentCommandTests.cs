using AutoMapper;
using Mosaico.Tests.Base;
using System;
using System.Collections.Generic;
using Mosaico.Events.Base;
using Moq;
using Mosaico.SDK.ProjectManagement.Abstractions;
using Autofac;
using Mosaico.Persistence.SqlServer.Contexts.DocumentContext;
using System.Threading;
using FluentValidation;
using Mosaico.SDK.ProjectManagement.Models;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.CreateProjectDocument;
using Mosaico.Application.DocumentManagement.Exceptions;
using Mosaico.Domain.DocumentManagement.Entities;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.DocumentManagement.Tests
{
    public class CreateProjectDocumentCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;
        private Mock<IProjectManagementClient> _projectManagementClient;
        protected override List<Profile> Profiles => new List<Profile>()
        {
            new DocumentManagementMapperProfile()
        };

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            
            RegisterContext<DocumentContext>(builder);
            
            _eventFactory = new CloudEventFactory();
            builder.RegisterInstance(_eventFactory).As<IEventFactory>();
            
            _eventPublisherMock = new Mock<IEventPublisher>();
            _eventPublisherMock.Setup(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>())).Verifiable();
            builder.RegisterInstance(_eventPublisherMock.Object).As<IEventPublisher>();
           
            _projectManagementClient = new Mock<IProjectManagementClient>();
            _projectManagementClient.Setup(c => c.GetProjectAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync(new MosaicoProject());
            builder.RegisterInstance(_projectManagementClient.Object).As<IProjectManagementClient>();

            builder.RegisterType<CreateProjectDocumentCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<CreateProjectDocumentCommandValidator>().As<IValidator<CreateProjectDocumentCommand>>();
        }

        [Test]
        public async Task ShouldCreateProjectDocument()
        {
            // Arrange
            var context = GetContext<DocumentContext>();
            var projectId = Guid.NewGuid();
            var command = new CreateProjectDocumentCommand
            {
                Title = "Test Document",
                ProjectId = projectId,
                IsMandatory = true
            };

            // Act
            var response = await SendAsync(command);

            // Assert
            response.Should().NotBe(Guid.Empty);

            var document = await context.ProjectDocuments.FirstOrDefaultAsync(pd => pd.Id == response);

            document.Should().NotBeNull();
            document.Title.Should().Be("Test Document");
            document.CreatedById.Should().Be(CurrentUserContext.UserId);
            document.ModifiedById.Should().Be(CurrentUserContext.UserId);
            document.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
            document.ModifiedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromSeconds(2));
            document.IsMandatory.Should().BeTrue();
            document.ProjectId.Should().Be(projectId);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCreateProjectDocumentWithoutTitle()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var command = new CreateProjectDocumentCommand
            {
                ProjectId = projectId
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
        public async Task ShouldNotCreateProjectDocumentWithoutProjectId()
        {
            // Arrange
            var command = new CreateProjectDocumentCommand
            {
                Title = "Test Document"
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
        public async Task ShouldNotCreateProjectDocumentIfProjectDoesNotExist()
        {
            _projectManagementClient.Setup(c => c.GetProjectAsync(It.IsAny<Guid>(), CancellationToken.None))
                .ReturnsAsync((MosaicoProject)default);

            // Arrange
            var command = new CreateProjectDocumentCommand
            {
                Title = "Test Document",
                ProjectId = Guid.NewGuid()
            };

            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<ProjectNotFoundException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotCreateProjectDocumentWithDuplicateTitle()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var context = GetContext<DocumentContext>();
            context.Add(new ProjectDocument
            {
                Title = "Test Document",
                ProjectId = projectId
            });
            await context.SaveChangesAsync(CancellationToken.None);
            
            var command = new CreateProjectDocumentCommand
            {
                Title = "Test Document",
                ProjectId = projectId
            };

            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<DuplicateDocumentTitleException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
    }
}
