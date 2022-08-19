using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Queries.GetProjectDocuments
{
    public class GetProjectDocumentsQueryValidator : AbstractValidator<GetProjectDocumentQuery>
    {
        public GetProjectDocumentsQueryValidator()
        {
            RuleFor(x => x.ProjectId).NotNull().NotEmpty();
        }
    }
}
