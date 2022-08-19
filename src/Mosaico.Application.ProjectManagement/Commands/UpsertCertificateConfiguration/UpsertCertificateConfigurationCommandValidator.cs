using FluentValidation;

namespace Mosaico.Application.ProjectManagement.Commands.UpsertCertificateConfiguration
{
    public class UpsertCertificateConfigurationCommandValidator : AbstractValidator<UpsertCertificateConfigurationCommand>
    {
        public UpsertCertificateConfigurationCommandValidator()
        {
            RuleFor(t => t.ProjectId).NotEmpty();
            RuleFor(t => t.Logo.IsValid() == true);
            RuleFor(t => t.Code.IsValid() == true);
            RuleFor(t => t.Date.IsValid() == true);
            RuleFor(t => t.Name.IsValid() == true);
            RuleFor(t => t.Tokens.IsValid() == true);
        }
    }
}