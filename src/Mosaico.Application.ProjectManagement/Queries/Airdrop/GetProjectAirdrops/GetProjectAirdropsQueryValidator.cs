using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetProjectAirdrops
{
    public class GetProjectAirdropsQueryValidator : AbstractValidator<GetProjectAirdropsQuery>
    {
        public GetProjectAirdropsQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}