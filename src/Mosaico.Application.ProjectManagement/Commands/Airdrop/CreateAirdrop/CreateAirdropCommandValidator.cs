using System;
using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.Airdrop.CreateAirdrop
{
    public class CreateAirdropCommandValidator : AbstractValidator<CreateAirdropCommand>
    {
        public CreateAirdropCommandValidator()
        {
            RuleFor(t => t.Name).NotEmpty().Length(3, 50);
            RuleFor(t => t.EndDate).Must((o, c) => c > o.StartDate);
            RuleFor(t => t.StartDate).Must((s) => s > DateTimeOffset.UtcNow);
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.TotalCap).GreaterThan(0);
            RuleFor(t => t.TokensPerParticipant).GreaterThan(0);
        }
    }
}