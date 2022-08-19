using Autofac;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Moq;
using Mosaico.Application.DocumentManagement.Commands.StoreFile;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.DocumentContext;
using Mosaico.Storage.Base;
using Mosaico.Tests.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValidationException = Mosaico.Validation.Base.Exceptions.ValidationException;

namespace Mosaico.Application.DocumentManagement.Tests
{
    public class StoreFileCommandTests : EFInMemoryTestBase
    {
        private Mock<IEventPublisher> _eventPublisherMock;
        private IEventFactory _eventFactory;
        private Mock<IStorageClient> _storageClient;
        private const string FileId = "File Id";
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

            _storageClient = new Mock<IStorageClient>();
            _storageClient.Setup(c => c.CreateAsync(It.IsAny<StorageObject>(), true, true))
                .ReturnsAsync(FileId);
            builder.RegisterInstance(_storageClient.Object).As<IStorageClient>();

            builder.RegisterType<StoreFileCommandHandler>().AsImplementedInterfaces();
            builder.RegisterType<StoreFileCommandValidator>().As<IValidator<StoreFileCommand>>();
        }

        [Test]
        public async Task ShouldCreateStoreFile()
        {
            // Arrange
            var command = new StoreFileCommand
            {
                Content = new byte[1],
                FileName = "File Name"
            };

            // Act
            var response = await SendAsync(command);

            // Assert
            response.Should().NotBeNull();
            response.Should().NotBe(string.Empty);
            response.Should().Be(FileId);

            _eventPublisherMock.Verify(p => p.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Once);
        }

        [Test]
        public async Task ShouldNotCreateStoreFileWithoutContent()
        {
            // Arrange
            var command = new StoreFileCommand
            {
                Content = default,
                FileName = "File Name"
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
        public async Task ShouldNotCreateStoreFileWithoutFileName()
        {
            // Arrange
            var command = new StoreFileCommand
            {
                Content = new byte[1],
                FileName = string.Empty
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
    }
}
