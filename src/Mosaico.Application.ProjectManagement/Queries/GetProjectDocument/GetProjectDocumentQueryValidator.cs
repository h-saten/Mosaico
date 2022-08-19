using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectDocument
{
    public class GetProjectDocumentQueryValidator : AbstractValidator<GetProjectDocumentQuery>
    {
        public GetProjectDocumentQueryValidator()
        {
            RuleFor(t => t.Language).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Type).NotEmpty();
        }
    }
}