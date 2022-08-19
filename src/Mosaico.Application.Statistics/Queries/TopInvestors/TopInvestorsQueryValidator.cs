using FluentValidation;
using Mosaico.Application.Statistics.Queries.VisitsPerDay;

namespace Mosaico.Application.Statistics.Queries.TopInvestors
{
    public class TopInvestorsQueryValidator : AbstractValidator<TopInvestorsQuery>
    {
        public TopInvestorsQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}