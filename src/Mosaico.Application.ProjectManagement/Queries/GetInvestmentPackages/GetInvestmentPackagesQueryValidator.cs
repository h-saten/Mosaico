using System.Linq;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetInvestmentPackages
{
    public class GetInvestmentPackagesQueryValidator : AbstractValidator<GetInvestmentPackagesQuery>
    {
        public GetInvestmentPackagesQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
            RuleFor(t => t.Language).Must(l => Base.Constants.Languages.All.Contains(l));
        }
    }
}