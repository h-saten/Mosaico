using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Queries.GetCertificateConfiguration
{
    public class GetCertificateConfigurationQueryValidator : AbstractValidator<GetCertificateConfigurationQuery>
    {
        public GetCertificateConfigurationQueryValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Language).NotEmpty();
        }
    }
}