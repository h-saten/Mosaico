using AutoMapper;
using Mosaico.Tests.Base;
using System;
using System.Collections.Generic;
using Mosaico.Events.Base;
using Moq;
using Autofac;
using Mosaico.Persistence.SqlServer.Contexts.DocumentContext;
using FluentValidation;
using System.Threading.Tasks;
using NUnit.Framework;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Mosaico.Application.DocumentManagement.Exceptions;
using Mosaico.Domain.DocumentManagement.Entities;
using Mosaico.Domain.DocumentManagement.Exceptions;
using Mosaico.Application.DocumentManagement.Commands.RemoveDocumentContent;
using System.Threading;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;
using Languages = Mosaico.Base.Constants.Languages;

namespace Mosaico.Application.DocumentManagement.Tests
{
    public class RemoveDocumentContentCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;
        private Guid documentId;
        private Guid documentContentId;
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

            builder.RegisterType<RemoveProjectDocumentCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<RemoveProjectDocumentCommandValidator>().As<IValidator<RemoveProjectDocumentCommand>>();
        }

        [SetUp]
        public async Task SetupBeforeEachTest()
        {
            var context = GetContext<DocumentContext>();
            var document = new Mock<DocumentBase>().Object;
            document.Title = "Document Title";
            context.Documents.Add(document);
            await context.SaveChangesAsync(CancellationToken.None);
            var documentContent = new DocumentContent
            {
                DocumentId = document.Id,
                Language = Languages.English,
                DocumentAddress = "Document Address"
            };
            context.DocumentContents.Add(documentContent);
            await context.SaveChangesAsync(CancellationToken.None);

            documentId = document.Id;
            documentContentId = documentContent.Id;
        }

        [Test]
        public async Task ShouldRemoveDocumentContent()
        {
            // Arrange
            var context = GetContext<DocumentContext>();

            var command = new RemoveProjectDocumentCommand
            {
                DocumentId = documentId,
                Language = Languages.English,
            };

            // Act
            await SendAsync(command);

            // Assert
            var documentContent = await context.DocumentContents.FirstOrDefaultAsync(dc => dc.Id == documentContentId);

            documentContent.Should().BeNull();

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotRemoveDocumentContentWithoutDocumentId()
        {
            // Arrange
            var command = new RemoveProjectDocumentCommand
            {
                DocumentId = Guid.Empty,
                Language = Languages.English,
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
        public async Task ShouldNotRemoveDocumentContentWithouLanguage()
        {
            // Arrange
            var command = new RemoveProjectDocumentCommand
            {
                DocumentId = documentId,
                Language = string.Empty,
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
        public async Task ShouldNotRemoveDocumentContentWithNotSupportedLanguage()
        {
            // Arrange
            var command = new RemoveProjectDocumentCommand
            {
                DocumentId = documentId,
                Language = "not-supported",
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
        public async Task ShouldNotRemoveDocumentContentIfDocumentNotExists()
        {
            // Arrange
            var command = new RemoveProjectDocumentCommand
            {
                DocumentId = Guid.NewGuid(),
                Language = Languages.English,
            };

            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<DocumentLanguageNotAvailableException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }

        [Test]
        public async Task ShouldNotRemoveDocumentContentIfLanguageIsNotAvailable()
        {
            // Arrange
            var command = new RemoveProjectDocumentCommand
            {
                DocumentId = documentId,
                Language = Languages.Polish,
            };

            // Act
            // Assert
            Func<Task> action = async () =>
            {
                await SendAsync(command);
            };
            await action.Should().ThrowAsync<DocumentLanguageNotAvailableException>()
                .Where(exception => exception.StatusCode == StatusCodes.Status404NotFound);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Never);
        }
    }
}
