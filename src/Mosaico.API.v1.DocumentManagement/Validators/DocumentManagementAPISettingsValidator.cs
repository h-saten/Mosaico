using FluentValidation;
using Mosaico.API.v1.DocumentManagement.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.v1.DocumentManagement.Validators
{
    public class DocumentManagementAPISettingsValidator : AbstractValidator<DocumentManagementAPISettings>
    {
        public DocumentManagementAPISettingsValidator()
        {
            RuleFor(s => s.MaximumFileSize_Bytes).GreaterThan(0).WithMessage("DocumentManagementAPI is misconfigured. MaximumFileSize must be greater than 0 bytes");
            RuleFor(s => s.PermittedExtensions).NotNull().NotEmpty().WithMessage("DocumentManagementAPI is misconfigured. PermittedExtensions can not be empty");
        }
    }
}
