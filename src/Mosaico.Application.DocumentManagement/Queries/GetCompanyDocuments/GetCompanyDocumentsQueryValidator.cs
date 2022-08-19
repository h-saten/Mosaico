using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.Application.DocumentManagement.Queries.GetCompanyDocuments
{
    public class GetCompanyDocumentsQueryValidator : AbstractValidator<GetCompanyDocumentQuery>
    {
        public GetCompanyDocumentsQueryValidator()
        {
            RuleFor(x => x.CompanyId).NotNull().NotEmpty();
        }
    }
}
