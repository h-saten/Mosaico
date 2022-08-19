using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.Identity.Commands.GenerateSmsConfirmationCode;
using Mosaico.Application.Identity.Exceptions;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Policies;
using Mosaico.Domain.Identity.Repositories;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Integration.Sms.Abstraction;
using Mosaico.Persistence.SqlServer.Contexts.Identity;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Identity.Tests.Commands
{
    public class GenerateSmsConfirmationCodeCommandHandlerTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new();

        private Mock<ILogger> _logger;
        private ICurrentUserContext _userContext;
        private Mock<ISmsSender> _smsSender;
        private Mock<IPhoneNumberConfirmationCodeGenerationPolicy> _policy;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            var context = RegisterContext<ApplicationDbContext>(builder);
            builder.RegisterModule(new IdentityApplicationModule());
            
            _logger = new Mock<ILogger>();
            builder.RegisterInstance(_logger.Object).AsImplementedInterfaces();
            
            _userContext = CreateCurrentUserContextMock();
            builder.RegisterInstance(_userContext).AsImplementedInterfaces();

            _smsSender = new Mock<ISmsSender>();
            _smsSender.Setup(x => x.SendAsync(It.IsAny<SmsMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SmsSentResult {Status = SmsStatus.OK});
            builder.RegisterInstance(_smsSender.Object).AsImplementedInterfaces();

            var phoneNumberConfirmationCodesRepository = new PhoneNumberConfirmationCodesRepository(context);
            builder.RegisterInstance(phoneNumberConfirmationCodesRepository).AsImplementedInterfaces();

            _policy = new Mock<IPhoneNumberConfirmationCodeGenerationPolicy>();
            builder.RegisterInstance(_policy.Object).AsImplementedInterfaces();
        }

        [Test]
        public void ShouldThrowExceptionWhenCannotGenerateCode()
        {
            _policy.Setup(x => x.CanGenerate(It.IsAny<string>(), It.IsAny<PhoneNumber>()))
                .ReturnsAsync(false);

            var command = new GenerateSmsConfirmationCodeCommand
            {
                PhoneNumber = "+48886163742",
                UserId = _userContext.UserId.ToString()
            };
            
            Assert.ThrowsAsync<ConfirmationCodeCannotBeGeneratedException>(async () => await SendAsync(command));
        } 

        [Test]
        public async Task ShouldSendCodeOnlyOnce()
        {
            var context = GetContext<ApplicationDbContext>();
            var policy = _policy;
            policy.Setup(x => x.CanGenerate(It.IsAny<string>(), It.IsAny<PhoneNumber>()))
                .ReturnsAsync(true);

            var command = new GenerateSmsConfirmationCodeCommand
            {
                PhoneNumber = "+48886163742",
                UserId = _userContext.UserId.ToString()
            };

            await SendAsync(command);
            
            _smsSender.Verify(x => x.SendAsync(It.IsAny<SmsMessage>(), It.IsAny<CancellationToken>()), Times.Exactly(1));

            var codesAmount = await context
                .PhoneNumberConfirmationCodes
                .CountAsync(x => x.UserId == _userContext.UserId.ToString());
            
            Assert.AreEqual(1, codesAmount);
        } 
    }
}