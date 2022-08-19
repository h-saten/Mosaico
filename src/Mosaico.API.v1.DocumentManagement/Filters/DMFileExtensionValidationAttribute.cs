using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Filters.FileValidation;
using Mosaico.API.v1.DocumentManagement.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.v1.DocumentManagement.Filters
{
    public class DMFileExtensionValidationAttribute : FileExtensionValidationAttribute
    {
        public DMFileExtensionValidationAttribute(DocumentManagementAPISettings settings) : 
            base(settings?.PermittedExtensions)
        {
        }
    }
}
