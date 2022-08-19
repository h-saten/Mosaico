using System.Linq;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.TokenPage.GetPageFaq
{
    public class GetPageFaqQueryValidator : AbstractValidator<GetPageFaqQuery>
    {
        public GetPageFaqQueryValidator()
        {
            RuleFor(c => c.Language).NotEmpty().Must(l => Base.Constants.Languages.All.Contains(l));
            RuleFor(c => c.PageId).NotEmpty();
        }
    }
}