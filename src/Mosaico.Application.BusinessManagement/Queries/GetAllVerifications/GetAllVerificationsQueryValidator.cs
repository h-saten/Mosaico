using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.GetAllVerifications
{
    public class GetAllVerificationsQueryValidator : AbstractValidator<GetAllVerificationsQuery>
    {
        public GetAllVerificationsQueryValidator()
        {
            RuleSet("default", () =>
            {
                RuleFor(q => q.Skip).GreaterThanOrEqualTo(0);
                RuleFor(q => q.Take).GreaterThan(0).LessThanOrEqualTo(100);
            });
        }
    }
}