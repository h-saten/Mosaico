using FluentValidation;

namespace Mosaico.Application.Statistics.Queries.VisitsPerDay
{
    public class VisitsPerDayQueryValidator : AbstractValidator<VisitsPerDayQuery>
    {
        public VisitsPerDayQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}