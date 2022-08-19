using FluentValidation;

namespace Mosaico.Application.Statistics.Queries.StatisticsSummary
{
    public class StatisticsSummaryQueryValidator : AbstractValidator<StatisticsSummaryQuery>
    {
        public StatisticsSummaryQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}