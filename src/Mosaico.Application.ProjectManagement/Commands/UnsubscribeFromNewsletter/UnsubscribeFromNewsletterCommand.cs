using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Mosaico.Application.ProjectManagement.Commands.UnsubscribeFromNewsletter
{
    public class UnsubscribeFromNewsletterCommand : IRequest
    {
        [JsonIgnore] 
        public Guid ProjectId { get; set; }

        [JsonIgnore] 
        public string UserId { get; set; }

        public string Email { get; set; }
        public string Code { get; set; }
    }
}