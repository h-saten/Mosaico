using FluentValidation;

namespace Mosaico.Application.Wallet.Queries.Project.GetProjectFee
{
    public class GetProjectFeeQueryValidator : AbstractValidator<GetProjectFeeQuery>
    {
        public GetProjectFeeQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}