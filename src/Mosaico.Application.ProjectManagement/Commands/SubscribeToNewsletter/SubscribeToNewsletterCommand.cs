using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Mosaico.Application.ProjectManagement.Commands.SubscribeToNewsletter
{
    public class SubscribeToNewsletterCommand : IRequest
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
        public string Email { get; set; }
        
        [JsonIgnore]
        public string UserId { get; set; }
    }
}