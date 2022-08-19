using FluentValidation;

namespace Mosaico.Application.BusinessManagement.Queries.GetCompanyDocuments
{
    public class GetCompanyDocumentsQueryValidator : AbstractValidator<GetCompanyDocumentsQuery>
    {
        public GetCompanyDocumentsQueryValidator()
        {
            RuleFor(t => t.CompanyId).NotEmpty();
        }
    }
}
