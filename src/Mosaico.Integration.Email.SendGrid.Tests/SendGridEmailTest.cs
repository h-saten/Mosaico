using Moq;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.Email.SendGridEmail.Configurations;
using Mosaico.Integration.Email.SendGridEmail.Exceptions;
using Mosaico.Integration.Email.SendGridEmail.Validators;
using Mosaico.Tests.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mosaico.Integration.Email.SendGridEmail.Tests
{
    public class SendGridEmailTest : TestBase
    {
        private SendGridEmailConfig _sendGridConfig;

        [SetUp]
        public void Setup()
        {
            _sendGridConfig = GetSettings<SendGridEmailConfig>(SendGridEmailConfig.SectionName);
        }

        [Test]
        public async Task ShouldSendEmail()
        {
            var email = new Abstraction.Email
            {
                Html = "<h1>Hello World</h1>",
                Recipients = new List<string>
                {
                    "development@mosaico.ai",                    
                },
                Subject = "Test Email"
            };
            var client = new SendGridEmailClient(_sendGridConfig, new EmailValidator(), null);
            var response = await client.SendAsync(email, It.IsAny<CancellationToken>());
            Assert.AreEqual(EmailStatus.OK, response.Status);
        }

        [Test]
        public void ShouldNotSendEmailIfNoHtml()
        {
            //Arrange
            var email = new Abstraction.Email
            {
                Recipients = new List<string>
                {
                    "development@mosaico.ai"
                },
                Subject = "Test Email"
            };
            var client = new SendGridEmailClient(_sendGridConfig, new EmailValidator(), null);
            //Act
            //Assert
            Assert.ThrowsAsync<EmailValidationException>(async () =>
            {
                var response = await client.SendAsync(email);
            });
        }

        [Test]
        public void ShouldNotSendEmailIfNoRecipients()
        {
            //Arrange
            var email = new Abstraction.Email
            {
                Html = "<h1>Hello World</h1>",
                Subject = "Test Email"
            };
            var client = new SendGridEmailClient(_sendGridConfig, new EmailValidator(), null);
            //Act
            //Assert
            Assert.ThrowsAsync<EmailValidationException>(async () =>
            {
                var response = await client.SendAsync(email);
            });
        }

        [Test]
        public void ShouldNotSendEmailIfNoSubject()
        {
            //Arrange
            var email = new Abstraction.Email
            {
                Html = "<h1>Hello World</h1>",
                Recipients = new List<string>
                {
                    "development@mosaico.ai"
                }
            };
            var client = new SendGridEmailClient(_sendGridConfig, new EmailValidator(), null);
            //Act
            //Assert
            Assert.ThrowsAsync<EmailValidationException>(async () =>
            {
                var response = await client.SendAsync(email);
            });
        }

        [Test]
        public void ShouldNotSendEmailIfSubjectTooLong()
        {
            //Arrange
            var email = new Abstraction.Email
            {
                Html = "<h1>Hello World</h1>",
                Recipients = new List<string>
                {
                    "development@mosaico.ai"
                },
                Subject = "Lose john poor same it case do year we. Full how way even the sigh. Extremely nor furniture fat questions now provision incommode"
            };
            var client = new SendGridEmailClient(_sendGridConfig, new EmailValidator(), null);
            //Act
            //Assert
            Assert.ThrowsAsync<EmailValidationException>(async () =>
            {
                var response = await client.SendAsync(email);
            });
        }

        [Test]
        public void ShouldNotSendEmailIfFromEmailEmpty()
        {
            //Arrange
            _sendGridConfig.FromEmail = string.Empty;
            var email = new Abstraction.Email
            {
                Html = "<h1>Hello World</h1>",
                Recipients = new List<string>
                {
                    "development@mosaico.ai"
                },
                Subject = "Lose john"
            };
            var client = new SendGridEmailClient(_sendGridConfig, new EmailValidator(), null);
            //Act
            //Assert
            Assert.ThrowsAsync<EmailValidationException>(async () =>
            {
                var response = await client.SendAsync(email);
            });
        }
    }
}
