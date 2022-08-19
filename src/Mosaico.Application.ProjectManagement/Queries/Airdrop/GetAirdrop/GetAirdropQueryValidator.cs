using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdrop
{
    public class GetAirdropQueryValidator : AbstractValidator<GetAirdropQuery>
    {
        public GetAirdropQueryValidator()
        {
            RuleFor(t => t.Id).NotEmpty();
        }
    }
}