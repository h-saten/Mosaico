using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetTemplateContent
{
    public class GetTemplateContentQueryValidator : AbstractValidator<GetTemplateContentQuery>
    {
        public GetTemplateContentQueryValidator()
        {
            RuleFor(t => t.Key).NotEmpty();
            RuleFor(t => t.Language).NotEmpty();
        }
    }
}