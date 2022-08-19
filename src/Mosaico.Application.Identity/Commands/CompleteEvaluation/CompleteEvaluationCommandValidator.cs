using FluentValidation;
using Mosaico.Application.Identity.DTOs;

namespace Mosaico.Application.Identity.Commands.CompleteEvaluation
{
    public class EvaluationResponseValidator : AbstractValidator<UserEvaluationQuestionDTO>
    {
        public EvaluationResponseValidator()
        {
            RuleFor(t => t.Title).Must(k => Constants.EvaluationQuestionKeys.All.Contains(k))
                .WithErrorCode("INVALID_EVALUATION_RESPONSES");
            RuleFor(t => t.Response).NotEmpty().WithErrorCode("INVALID_EVALUATION_RESPONSES");
        }
    }

    public class CompleteEvaluationCommandValidator : AbstractValidator<CompleteEvaluationCommand>
    {
        public CompleteEvaluationCommandValidator()
        {
            RuleFor(t => t.UserId).NotEmpty();
            RuleFor(t => t.Responses).NotNull().WithErrorCode("INVALID_EVALUATION_RESPONSES");
            RuleFor(t => t.Responses)
                .ForEach(r => r.SetValidator(new EvaluationResponseValidator()));
        }
    }
}