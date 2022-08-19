using System.Linq;
using FluentValidation;

namespace Mosaico.Application.DocumentManagement.Commands.TokenPage.DeletePageCover
{
    public class DeletePageCoverCommandValidator : AbstractValidator<DeletePageCoverCommand>
    {
        public DeletePageCoverCommandValidator()
        {
            RuleFor(t => t.PageId).NotEmpty();
            RuleFor(t => t.Language).Must(l => Mosaico.Base.Constants.Languages.All.Contains(l));
        }
    }
}