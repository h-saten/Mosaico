using MediatR;

namespace Mosaico.Application.ProjectManagement.Queries.ProjectPreValidation
{
    public class ProjectPreValidationQuery : IRequest<ProjectPreValidationQueryResponse>
    {
        public string Title { get; set; }
        public string Slug { get; set; }
    }
}