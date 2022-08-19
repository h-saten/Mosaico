using System.Linq;
using FluentValidation;
using Mosaico.Application.ProjectManagement.DTOs;

namespace Mosaico.Application.ProjectManagement.Validators
{
    public class UpdatePageValidator : AbstractValidator<UpdatePageDTO>
    {
        public UpdatePageValidator()
        {
            // RuleFor(c => c.ShortDescription).Must(t =>
            // {
            //     return t.Values.All(v => v.Length is > 3 and <= 450);
            // }).When(c => c.ShortDescription != null && c.ShortDescription.Any());
        }
    }
}