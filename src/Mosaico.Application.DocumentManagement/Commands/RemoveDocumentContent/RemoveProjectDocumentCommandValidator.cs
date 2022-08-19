using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Mosaico.Base.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Commands.RemoveDocumentContent
{
    public class RemoveProjectDocumentCommandValidator : AbstractValidator<RemoveProjectDocumentCommand>
    {
        public RemoveProjectDocumentCommandValidator()
        {
            RuleFor(x => x.DocumentId).NotNull().NotEmpty();
            RuleFor(x => x.ProjectId).NotNull().NotEmpty();
            RuleFor(x => x.Language).NotNull().NotEmpty();
            RuleFor(x => x.Language).Must(l => typeof(Mosaico.Base.Constants.Languages).GetPublicConstants().Contains(l)).WithMessage("Language is not supported");
        }
    }
}
