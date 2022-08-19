using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.Airdrop.GetAirdropParticipants
{
    public class GetAirdropParticipantsQueryValidator : AbstractValidator<GetAirdropParticipantsQuery>
    {
        public GetAirdropParticipantsQueryValidator()
        {
            RuleFor(t => t.AirdropId).NotEmpty();
            RuleFor(t => t.ProjectId).NotEmpty();
        }
    }
}