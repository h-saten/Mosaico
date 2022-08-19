using FluentValidation;

namespace Mosaico.Application.Features.Queries.GetAllEntitySettingsQuery
{
    public class GetAllSettingsQueryValidator : AbstractValidator<GetAllEntitySettingsQuery>
    {
        public GetAllSettingsQueryValidator()
        {
            RuleFor(t => t.Category).NotEmpty();
        }
    }
}