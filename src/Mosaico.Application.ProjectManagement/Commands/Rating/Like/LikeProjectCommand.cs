using System;
using MediatR;

namespace Mosaico.Application.ProjectManagement.Commands.Rating.Like
{
    public class LikeProjectCommand : IRequest
    {
        public Guid ProjectId { get; set; }
    }
}