using Microsoft.AspNetCore.Mvc.Filters;
using Mosaico.API.Base.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.Base.Filters.FileValidation
{
    public class FileSizeValidationAttribute : ActionFilterAttribute
    {
        private readonly long _sizeLimit;
        public FileSizeValidationAttribute(long sizeLimit)
        {
            _sizeLimit = sizeLimit;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var files = context.HttpContext.Request.Form.Files;
            foreach (var file in files)
                if (file.Length > _sizeLimit)
                    throw new FileSizeLimitExceededException(file.Length, _sizeLimit);
        }
    }
}
