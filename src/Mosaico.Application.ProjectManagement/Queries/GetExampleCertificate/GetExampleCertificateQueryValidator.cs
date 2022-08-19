using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetExampleCertificate
{
    public class GetExampleCertificateQueryValidator : AbstractValidator<GetExampleCertificateQuery>
    {
        public GetExampleCertificateQueryValidator()
        {
            RuleFor(q => q.ProjectId).NotEmpty();
            RuleFor(q => q.Language).NotEmpty();
        }
    }
}