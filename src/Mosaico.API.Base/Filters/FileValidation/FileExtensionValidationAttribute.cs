using Microsoft.AspNetCore.Mvc.Filters;
using Mosaico.API.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.Base.Filters.FileValidation
{
    public class FileExtensionValidationAttribute : ActionFilterAttribute
    {
        private readonly string[] _permittedExtensions;
        public FileExtensionValidationAttribute(params string[] permittedExtensions)
        {
            _permittedExtensions = permittedExtensions.Select(x => x.ToLowerInvariant()).ToArray();
        }
        public FileExtensionValidationAttribute(IEnumerable<string> permittedExtensions)
        {
            _permittedExtensions = permittedExtensions.ToArray();
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var files = context.HttpContext.Request.Form.Files;
            foreach (var file in files)
            {
                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(ext) || !_permittedExtensions.Any(x => x.Equals(ext)))
                    throw new NotPermittedFileExtensionException(file.Name);
            }
        }
    }
}
