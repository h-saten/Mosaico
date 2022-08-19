using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Mosaico.Integration.Email.Abstraction;
using Mosaico.Integration.Email.EmailLabs.Configurations;
using Mosaico.Integration.Email.EmailLabs.Exceptions;
using Mosaico.Integration.Email.EmailLabs.Models;
using Mosaico.Integration.Email.EmailLabs.Tests.Helpers;
using Mosaico.Integration.Email.EmailLabs.Validators;
using Mosaico.Tests.Base;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;

namespace Mosaico.Integration.Email.EmailLabs.Tests
{
    public class EmailLabsTests : TestBase
    {
        private EmailLabsConfig _emailLabsConfig;
        
        [SetUp]
        public void Setup()
        {
            _emailLabsConfig = GetSettings<EmailLabsConfig>(EmailLabsConfig.SectionName);
        }

        [Test]
        public async Task ShouldSendEmail()
        {
            //Arrange
            var email = new Abstraction.Email
            {
                Html = "<h1>Hello World</h1>",
                Recipients = new List<string>
                {
                    "development@mosaico.ai"
                },
                Subject = "Test Email"
            };

            var emailLabsResponse = new EmailLabsResponse
            {
                Code = 200,
                Status = "success",
                Message = "Message sent",
                RequestId = Guid.NewGuid().ToString()
            };
            
            var clientResponse = new Mock<IRestResponse<object>>();
            clientResponse.Setup(c => c.StatusCode).Returns(HttpStatusCode.OK);
            clientResponse.Setup(c => c.Content).Returns(JsonConvert.SerializeObject(emailLabsResponse));
            clientResponse.Setup(c => c.IsSuccessful).Returns(true);

            var interceptor = new TestRestClientInterceptor();
            interceptor.Client
                .Setup(i => i.ExecutePostAsync<object>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse.Object)
                .Verifiable();
            
            var client = new EmailLabsClient(_emailLabsConfig, new []{interceptor}, new EmailValidator(), null);
            //Act
            var response = await client.SendAsync(email, It.IsAny<CancellationToken>());
            //Assert
            Assert.AreEqual(EmailStatus.OK, response.Status);
            interceptor.Client.Verify(p => p.ExecutePostAsync<object>(It.IsAny<RestRequest>(), It.IsAny<CancellationToken>()), Times.Once);
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
            var client = new EmailLabsClient(_emailLabsConfig, null, new EmailValidator(), null);
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
            var client = new EmailLabsClient(_emailLabsConfig, null, new EmailValidator(), null);
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
            var client = new EmailLabsClient(_emailLabsConfig, null, new EmailValidator(), null);
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
            var client = new EmailLabsClient(_emailLabsConfig, null, new EmailValidator(), null);
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
            _emailLabsConfig.FromEmail = string.Empty;
            var email = new Abstraction.Email
            {
                Html = "<h1>Hello World</h1>",
                Recipients = new List<string>
                {
                    "development@mosaico.ai"
                },
                Subject = "Lose john"
            };
            var client = new EmailLabsClient(_emailLabsConfig, null, new EmailValidator(), null);
            //Act
            //Assert
            Assert.ThrowsAsync<EmailValidationException>(async () =>
            {
                var response = await client.SendAsync(email);
            });
        }
    }
}