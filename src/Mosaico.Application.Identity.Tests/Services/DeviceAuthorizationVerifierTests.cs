using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using FizzWare.NBuilder;
using Microsoft.EntityFrameworkCore;
using Moq;
using Mosaico.Application.Identity.Abstractions;
using Mosaico.Application.Identity.DTOs;
using Mosaico.Application.Identity.Services;
using Mosaico.Authorization.Base;
using Mosaico.Domain.Identity.Abstractions;
using Mosaico.Domain.Identity.Entities;
using Mosaico.Domain.Identity.Policies;
using Mosaico.Domain.Identity.Repositories;
using Mosaico.Domain.Identity.ValueObjects;
using Mosaico.Integration.Sms.Abstraction;
using Mosaico.Persistence.SqlServer.Contexts.Identity;
using Mosaico.Tests.Base;
using NUnit.Framework;
using Serilog;

namespace Mosaico.Application.Identity.Tests.Services
{
    public class DeviceAuthorizationVerifierTests : EFInMemoryTestBase
    {
        protected override List<Profile> Profiles => new();

        private Mock<ILogger> _logger;
        private ICurrentUserContext _userContext;
        private Mock<ISmsSender> _smsSender;
        private Mock<IIdentityEmailService> _emailService;
        private Mock<IPhoneNumberConfirmationCodeGenerationPolicy> _policy;
        private Mock<ISecurityCodeRepository> _securityCodeRepository;

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            base.RegisterDependencies(builder);
            RegisterContext<ApplicationDbContext>(builder);
            builder.RegisterModule(new IdentityApplicationModule());
            
            _logger = new Mock<ILogger>();
            builder.RegisterInstance(_logger.Object).AsImplementedInterfaces();
            
            _userContext = CreateCurrentUserContextMock();
            builder.RegisterInstance(_userContext).AsImplementedInterfaces();

            _smsSender = new Mock<ISmsSender>();
            _smsSender.Setup(x => x.SendAsync(It.IsAny<SmsMessage>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new SmsSentResult {Status = SmsStatus.OK});
            builder.RegisterInstance(_smsSender.Object).AsImplementedInterfaces();
            
            var identityEmailService = new Mock<IIdentityEmailService>();
            identityEmailService.Setup(x =>
                    x.SendDeviceAuthorizationCode(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _emailService = identityEmailService;
            
            var securityCodeRepository = new Mock<ISecurityCodeRepository>();
            var code = Builder<SecurityCode>.CreateNew().Build();
            securityCodeRepository.Setup(x =>
                    x.CreateCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(code);
            _securityCodeRepository = securityCodeRepository;

            _policy = new Mock<IPhoneNumberConfirmationCodeGenerationPolicy>();
            builder.RegisterInstance(_policy.Object).AsImplementedInterfaces();
        }

        private ApplicationUser AddUser()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var phoneNumber = "+48886163743";
            var userId = _userContext.UserId;
            var user = Builder<ApplicationUser>.CreateNew().Build();
            user.Id = userId;
            user.PhoneNumber = phoneNumber;
            dbContext.Users.Add(user);
            return user;
        }
        
        private ApplicationUser UserWithVerifiedPhoneNumber()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var phoneNumber = new PhoneNumber("+48886163742").Value;
            var userId = _userContext.UserId;
            var user = Builder<ApplicationUser>.CreateNew().Build();
            user.Id = userId;
            user.PhoneNumber = phoneNumber;
            user.PhoneNumberConfirmed = true;
            dbContext.Users.Add(user);
            return user;
        }

        private AuthorizedDevice AddAuthorizedDevice(ApplicationUser user)
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var authorizedDevice = Builder<AuthorizedDevice>.CreateNew().Build();
            authorizedDevice.User = user;
            authorizedDevice.AgentInfo = "Firefox";
            authorizedDevice.IP = "192.168.88.254";
            dbContext.AuthorizedDevices.Add(authorizedDevice);
            return authorizedDevice;
        }
        
        [Test]
        public async Task ShouldAuthorizeSuccessfully()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var securityCodeRepository = new Mock<ISecurityCodeRepository>();
            var phoneNumberConfirmationCodeRepository = new Mock<IPhoneNumberConfirmationCodesRepository>();
            var logger = new Mock<ILogger>();

            var user = AddUser();
            var authorizedDevice = AddAuthorizedDevice(user);

            await dbContext.SaveChangesAsync();

            var sut = new DeviceAuthorizationVerifier(
                dbContext,
                securityCodeRepository.Object,
                _emailService.Object,
                _smsSender.Object,
                phoneNumberConfirmationCodeRepository.Object,
                logger.Object
                );

            var result = await sut.VerifyAsync(x =>
            {
                x.User = user;
                x.AgentInfo = authorizedDevice.AgentInfo;
                x.AuthorizationCode = "code1234";
                x.IP = authorizedDevice.IP;
            }, new CancellationToken());

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.VerificationType);
        } 
        
        [Test]
        public async Task ShouldGenerateNewSmsCode()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var securityCodeRepository = new Mock<ISecurityCodeRepository>();
            var phoneNumberConfirmationCodeRepository = new PhoneNumberConfirmationCodesRepository(dbContext);
            var logger = new Mock<ILogger>();

            var user = UserWithVerifiedPhoneNumber();

            await dbContext.SaveChangesAsync();

            var sut = new DeviceAuthorizationVerifier(
                dbContext,
                securityCodeRepository.Object,
                _emailService.Object,
                _smsSender.Object,
                phoneNumberConfirmationCodeRepository,
                logger.Object
                );

            var result = await sut.VerifyAsync(x =>
            {
                x.User = user;
                x.AgentInfo = "Firefox";
                x.AuthorizationCode = null;
                x.IP = "192.168.88.200";
            }, new CancellationToken());
            
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.VerificationType, DeviceVerificationTypeDTO.Sms);

            var userCodes = await dbContext
                .PhoneNumberConfirmationCodes
                .CountAsync(x => x.UserId == user.Id && x.Used == false);
            
            Assert.IsTrue(userCodes == 1);
        } 
        
        [Test]
        public async Task ShouldGenerateNewEmailCode()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var user = AddUser();
            var phoneNumberConfirmationCodeRepository = new Mock<IPhoneNumberConfirmationCodesRepository>();
            var logger = new Mock<ILogger>();
            var securityCodeRepository = new Mock<ISecurityCodeRepository>();
            
            var code = Builder<SecurityCode>.CreateNew().Build();
            code.UserId = user.Id;
            dbContext.SecurityCodes.Add(code);
            securityCodeRepository.Setup(x =>
                    x.CreateCodeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(code);
            _securityCodeRepository = securityCodeRepository;
            
            await dbContext.SaveChangesAsync();

            var sut = new DeviceAuthorizationVerifier(
                dbContext,
                _securityCodeRepository.Object,
                _emailService.Object,
                _smsSender.Object,
                phoneNumberConfirmationCodeRepository.Object,
                logger.Object
                );

            var result = await sut.VerifyAsync(x =>
            {
                x.User = user;
                x.AgentInfo = "Firefox";
                x.AuthorizationCode = null;
                x.IP = "192.168.88.200";
            }, new CancellationToken());
            
            Assert.IsFalse(result.Success);
            Assert.AreEqual(result.VerificationType, DeviceVerificationTypeDTO.Email);

            var userCodes = await dbContext
                .SecurityCodes
                .CountAsync(x => x.UserId == user.Id && x.IsUsed == false);
            
            Assert.IsTrue(userCodes == 1);
        } 
        
        [Test]
        public async Task ShouldVerifyPassedSmsCodeSuccessfully()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var user = UserWithVerifiedPhoneNumber();

            var logger = new Mock<ILogger>();
            var phoneNumberConfirmationCodeRepository = new PhoneNumberConfirmationCodesRepository(dbContext);
            var code = await phoneNumberConfirmationCodeRepository.CreateCodeAsync(user.Id, new PhoneNumber(user.PhoneNumber));
            
            await dbContext.SaveChangesAsync();

            var sut = new DeviceAuthorizationVerifier(
                dbContext,
                _securityCodeRepository.Object,
                _emailService.Object,
                _smsSender.Object,
                phoneNumberConfirmationCodeRepository,
                logger.Object);

            var agentInfo = "Firefox";
            var ip = "192.168.88.200";
            var result = await sut.VerifyAsync(x =>
            {
                x.User = user;
                x.AgentInfo = agentInfo;
                x.AuthorizationCode = code.Code;
                x.IP = ip;
            }, new CancellationToken());
            
            Assert.IsTrue(result.Success);

            var userCodes = await dbContext
                .PhoneNumberConfirmationCodes
                .CountAsync(x => x.UserId == user.Id && x.Code == code.Code && x.Used == true);
            
            var authorizedDeviceEntityExist = await dbContext
                .AuthorizedDevices
                .AsNoTracking()
                .Where(x => 
                    x.User.Id == user.Id 
                    && x.AgentInfo == agentInfo
                    && x.IP == ip)
                .AnyAsync();
            
            Assert.IsTrue(userCodes == 1);
            Assert.IsTrue(authorizedDeviceEntityExist);
        } 
        
        [Test]
        public async Task ShouldVerifyPassedEmailCodeSuccessfully()
        {
            var dbContext = GetContext<ApplicationDbContext>();
            var user = AddUser();

            var phoneNumberConfirmationCodeRepository = new PhoneNumberConfirmationCodesRepository(dbContext);
            var securityCodeRepo = new SecurityCodeRepository(dbContext);
            var logger = new Mock<ILogger>();
            var code = Builder<SecurityCode>.CreateNew().Build();
            code.ExpiresAt = DateTimeOffset.UtcNow.AddMinutes(1);
            code.UserId = user.Id;
            code.Context = SecurityCode.DeviceAuthorizationContext;
            dbContext.SecurityCodes.Add(code);
            
            await dbContext.SaveChangesAsync();

            var sut = new DeviceAuthorizationVerifier(
                dbContext,
                securityCodeRepo,
                _emailService.Object,
                _smsSender.Object,
                phoneNumberConfirmationCodeRepository,
                logger.Object
                );

            var agentInfo = "Firefox";
            var ip = "192.168.88.200";
            var result = await sut.VerifyAsync(x =>
            {
                x.User = user;
                x.AgentInfo = agentInfo;
                x.AuthorizationCode = code.Code;
                x.IP = ip;
            }, new CancellationToken());
            
            Assert.IsTrue(result.Success);

            var userCodes = await dbContext
                .SecurityCodes
                .CountAsync(x => x.UserId == user.Id && x.Code == code.Code && x.IsUsed == true);
            
            var authorizedDeviceEntityExist = await dbContext
                .AuthorizedDevices
                .AsNoTracking()
                .Where(x => 
                    x.User.Id == user.Id 
                    && x.AgentInfo == agentInfo
                    && x.IP == ip)
                .AnyAsync();
            
            Assert.IsTrue(userCodes == 1);
            Assert.IsTrue(authorizedDeviceEntityExist);
        }
        
        [Test]
        public async Task ShouldSendViaEmailWhenSmsSendingErrorOccured()
        {
            // arrange
            var dbContext = GetContext<ApplicationDbContext>();
            var phoneNumberConfirmationCodeRepository = new PhoneNumberConfirmationCodesRepository(dbContext);
            var logger = new Mock<ILogger>();

            var user = UserWithVerifiedPhoneNumber();

            await dbContext.SaveChangesAsync();

            _smsSender.Setup(x => x.SendAsync(It.IsAny<SmsMessage>(), It.IsAny<CancellationToken>()))
                .Throws<Exception>();
            
            var sut = new DeviceAuthorizationVerifier(
                dbContext,
                _securityCodeRepository.Object,
                _emailService.Object,
                _smsSender.Object,
                phoneNumberConfirmationCodeRepository,
                logger.Object
            );

            // act
            var result = await sut.VerifyAsync(x =>
            {
                x.User = user;
                x.AgentInfo = "Firefox";
                x.AuthorizationCode = null;
                x.IP = "192.168.88.200";
            }, new CancellationToken());
            
            // assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual(DeviceVerificationTypeDTO.Email, result.VerificationType);
        } 
    }
}