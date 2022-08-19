using FluentValidation;

namespace Mosaico.Application.Statistics.Queries.DailyRaisedCapital
{
    public class DailyRaisedCapitalQueryValidator : AbstractValidator<DailyRaisedCapitalQuery>
    {
        public DailyRaisedCapitalQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.FromMonthAgo).GreaterThanOrEqualTo(0);
        }
    }
}