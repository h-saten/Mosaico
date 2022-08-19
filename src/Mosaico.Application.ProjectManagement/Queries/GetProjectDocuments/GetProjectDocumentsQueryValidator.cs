using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetProjectDocuments
{
    public class GetProjectDocumentsQueryValidator : AbstractValidator<GetProjectDocumentsQuery>
    {
        public GetProjectDocumentsQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}