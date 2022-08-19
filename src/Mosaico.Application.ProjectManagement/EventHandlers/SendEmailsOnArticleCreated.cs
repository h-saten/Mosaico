using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mosaico.Application.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Abstractions;
using Mosaico.Domain.ProjectManagement.Entities;
using Mosaico.Domain.ProjectManagement.Exceptions;
using Mosaico.Events.Base;
using Mosaico.Events.ProjectManagement;
using Mosaico.Integration.Email.Abstraction;

namespace Mosaico.Application.ProjectManagement.EventHandlers
{
    [EventInfo(nameof(SendEmailsOnArticleCreated), "projects:api")]
    [EventTypeFilter(typeof(ProjectArticleCreatedEvent))]
    public class SendEmailsOnArticleCreated : EventHandlerBase
    {
        private readonly IProjectDbContext _projectDb;
        private readonly IEmailSender _emailSender;


        public SendEmailsOnArticleCreated(IProjectDbContext projectDb, IEmailSender emailSender)
        {
            _projectDb = projectDb;
            _emailSender = emailSender;
        }

        public override async Task HandleAsync(CloudEvent @event)
        {
            var projectEvent = @event?.GetData<ProjectArticleCreatedEvent>();
            if (projectEvent != null)
            {
                var email = new Email
                {
                    Html = "New article was added to the project, visit ",
                    Recipients = projectEvent.Emails,
                    Subject = "New article"
                };
                await _emailSender.SendAsync(email);
            }
        }
    }
}