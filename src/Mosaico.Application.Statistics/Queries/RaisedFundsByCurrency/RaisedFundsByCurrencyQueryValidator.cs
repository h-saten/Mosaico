using FluentValidation;

namespace Mosaico.Application.Statistics.Queries.RaisedFundsByCurrency
{
    public class RaisedFundsByCurrencyQueryValidator : AbstractValidator<RaisedFundsByCurrencyQuery>
    {
        public RaisedFundsByCurrencyQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}