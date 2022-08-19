using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyDocument
{
    internal class GetCompanyDocumentQueryValidator:AbstractValidator<GetCompanyDocumentQuery>
    {
        public GetCompanyDocumentQueryValidator()
        {
            RuleFor(t => t.Language).NotEmpty();
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}
