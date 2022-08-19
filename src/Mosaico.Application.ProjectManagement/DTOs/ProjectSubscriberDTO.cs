using System;

namespace Mosaico.Application.ProjectManagement.DTOs
{
    public class ProjectSubscriberDTO
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? SubscribedAt { get; set; }
    }
}