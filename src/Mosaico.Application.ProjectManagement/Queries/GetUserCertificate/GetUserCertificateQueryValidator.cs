using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetUserCertificate
{
    public class GetUserCertificateQueryValidator : AbstractValidator<GetUserCertificateQuery>
    {
        public GetUserCertificateQueryValidator()
        {
            RuleFor(q => q.ProjectId).NotEmpty();
            RuleFor(q => q.UserId).NotEmpty();
        }
    }
}