using System;
using MediatR;
using Mosaico.Authorization.Base;

namespace Mosaico.Application.ProjectManagement.Commands.CreateProject
{
    [ShouldCompleteEvaluation]
    public class CreateProjectCommand : IRequest<Guid>
    {
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public Guid CompanyId { get; set; }
    }
}