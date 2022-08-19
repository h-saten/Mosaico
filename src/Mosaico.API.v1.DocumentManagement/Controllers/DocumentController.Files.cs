using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.API.v1.DocumentManagement.Filters;
using Mosaico.Application.DocumentManagement.Commands.StoreFile;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    public partial class DocumentController
    {
        [HttpPost("files")]
        [TypeFilter(typeof(DMFileExtensionValidationAttribute))]
        [TypeFilter(typeof(DMFileSizeValidationAttribute))]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> StoreFile([FromForm] IFormFile[] files)
        {
            var file = files.FirstOrDefault();
            var command = new StoreFileCommand();
            if (file != null && file.Length > 0)
            {
                command.FileName = file.FileName;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    command.Content = stream.ToArray();
                }
            }

            var id = await _mediator.Send(command);

            return new SuccessResult(id) { StatusCode = 201 };
        }
    }
}
