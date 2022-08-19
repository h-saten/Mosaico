using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.Identity.Commands.VerifyPhoneNumber;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Repositories;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Events.Base;
using Mosaico.Persistence.SqlServer.Contexts.Identity;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Identity.Tests.Commands
{
    public class VerifyPhoneNumberCommandHandlerTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new();

        private Mock<ILogger> _logger;
        private ICurrentUserContext _userContext;
        private Mock<UserManager<ApplicationUser>> _userManager;
        private Mock<IEventPublisher> _eventPublisher;
        private Mock<IEventFactory> _eventFactory;
        private Mock<ISecurityCodeRepository> _securityCodeRepoMock;
        private IPhoneNumberConfirmationCodesRepository _phoneNumberConfirmationCodesRepository;
        private ISecurityCodeRepository _securityCodeRepository;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            var context = RegisterContext<ApplicationDbContext>(builder);
            builder.RegisterModule(new IdentityApplicationModule());
            _userContext = CreateCurrentUserContextMock();
            _logger = new Mock<ILogger>();
            _userManager = new Mock<UserManager<ApplicationUser>>(new Mock<IUserStore<ApplicationUser>>().Object, null, null, null, null, null, null, null, null);
            _eventPublisher = new Mock<IEventPublisher>();
            _eventFactory = new Mock<IEventFactory>();
            _eventFactory.Setup(m => m.CreateEvent(It.IsAny<string>(), It.IsAny<object>())).Returns(new CloudEvent());
            _phoneNumberConfirmationCodesRepository = new PhoneNumberConfirmationCodesRepository(context);
            _securityCodeRepoMock = new Mock<ISecurityCodeRepository>();
            _securityCodeRepoMock.Setup(m => m.CreateCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        }

        [Test]
        public void ShouldThrowExceptionWhenCodeIsInvalid()
        {
            var context = GetContext<ApplicationDbContext>();
            
            var sut = new VerifyPhoneNumberCommandHandler(
                context,
                _userManager.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _securityCodeRepoMock.Object,
                _phoneNumberConfirmationCodesRepository,
                _logger.Object
            );

            var command = new VerifyPhoneNumberCommand
            {
                PhoneNumber = "+48886163742",
                UserId = _userContext.UserId,
                ConfirmationCode = "testcode"
            };

            Assert.ThrowsAsync<InvalidPhoneNumberConfirmationCodeException>(async () => await sut.Handle(command, new CancellationToken()));
        }

        [Test]
        public async Task ShouldAddPhoneNumberToUserAccount()
        {
            var phoneNumber = "+48886163743";
            var userId = _userContext.UserId;
            var context = GetContext<ApplicationDbContext>();
            
            var user = Builder<ApplicationUser>.CreateNew().Build();
            user.Id = _userContext.UserId;
            user.PhoneNumber = null;
            context.Users.Add(user);
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(user);
            
            var code = new PhoneNumberConfirmationCode(userId, new PhoneNumber(phoneNumber), "testcode")
            {
                CreatedAt = DateTimeOffset.UtcNow
            };
            context.PhoneNumberConfirmationCodes.Add(code);
            
            await context.SaveChangesAsync();

            var command = new VerifyPhoneNumberCommand
            {
                PhoneNumber = phoneNumber,
                UserId = userId,
                ConfirmationCode = code.Code
            };

            var sut = new VerifyPhoneNumberCommandHandler(
                context,
                _userManager.Object,
                _eventPublisher.Object,
                _eventFactory.Object,
                _securityCodeRepoMock.Object,
                _phoneNumberConfirmationCodesRepository,
                _logger.Object
            );

            await sut.Handle(command, new CancellationToken());
            
            _eventPublisher.Verify(x => x.PublishAsync(It.IsAny<string>(), It.IsAny<CloudEvent>()), Times.Exactly(1));
            var codeEntity = context
                .PhoneNumberConfirmationCodes
                .ToList()
                .SingleOrDefault(x => x.Code == code.Code);
            
            var userEntity = await context
                .Users
                .Where(x => x.Id == userId)
                .SingleOrDefaultAsync();
            
            Assert.IsTrue(codeEntity.Used);
            Assert.AreEqual(phoneNumber, userEntity.PhoneNumber);
        }
    }
}