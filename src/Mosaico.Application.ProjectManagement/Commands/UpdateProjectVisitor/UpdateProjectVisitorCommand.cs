using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Mosaico.Application.ProjectManagement.Commands.UpdateProjectVisitor
{
    public class UpdateProjectVisitorCommand: IRequest<Guid>
    {
        [JsonIgnore]
        public Guid ProjectId { get; set; }
    }
}
