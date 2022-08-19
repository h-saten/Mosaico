using Autofac;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.DocumentManagement.Exceptions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Domain.DocumentManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.DocumentContext;
using Mosaico.Tests.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.DeleteProjectDocument;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.DocumentManagement.Tests
{
    public class DeleteProjectDocumentCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;

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

            builder.RegisterType<DeleteProjectDocumentCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<DeleteProjectDocumentCommandValidator>().As<IValidator<DeleteProjectDocumentCommand>>();
        }

        [Test]
        public async Task ShouldDeleteProjectDocument()
        {
            // Arrange
            var context = GetContext<DocumentContext>();
            var addedDocument = new ProjectDocument
            {
                Title = "Document Title",
                ProjectId = Guid.NewGuid()
            };
            context.ProjectDocuments.Add(addedDocument);
            await context.SaveChangesAsync(CancellationToken.None);

            var command = new DeleteProjectDocumentCommand
            {
                Id = addedDocument.Id
            };

            // Act
            await SendAsync(command);

            // Assert
            var deletedDocument = await context.ProjectDocuments.FirstOrDefaultAsync(pd => pd.Id == addedDocument.Id);

            deletedDocument.Should().BeNull();

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotDeleteProjectDocumentWithoutId()
        {
            // Arrange
            var command = new DeleteProjectDocumentCommand
            {
                Id = Guid.Empty
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
        public async Task ShouldNotDeleteProjectDocumentIfDocumentDoesNotExists()
        {
            // Arrange
            var command = new DeleteProjectDocumentCommand
            {
                Id = Guid.NewGuid()
            };

            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<DocumentNotFoundException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotDeleteMandatoryProjectDocument()
        {
            // Arrange
            var context = GetContext<DocumentContext>();
            var addedDocument = new ProjectDocument
            {
                Title = "Added Document Title",
                ProjectId = new Guid("d93a09a4-4e8e-11ec-81d3-0242ac130003"),
                IsMandatory = true
            };
            context.ProjectDocuments.Add(addedDocument);
            await context.SaveChangesAsync(CancellationToken.None);

            var command = new DeleteProjectDocumentCommand
            {
                Id = addedDocument.Id
            };

            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<DocumentIsMandatoryException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status400BadRequest);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
    }
}
