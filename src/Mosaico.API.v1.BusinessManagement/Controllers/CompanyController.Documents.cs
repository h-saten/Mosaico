using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.BusinessManagement.Commands.UploadCompanyDocuments;
using Mosaico.Application.BusinessManagement.Queries.GetCompanyDocuments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mosaico.API.v1.BusinessManagement.Controllers
{
    public partial class CompanyController
    {
        [HttpGet("{id}/getdocuments")]
        [ProducesResponseType(typeof(SuccessResult<GetCompanyDocumentsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCompanyDocuments([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetCompanyDocumentsQuery
            {
                CompanyId = id
            });
            return new SuccessResult(response);
        }

        [HttpPost("{id}/document/upload")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UploadCompanyDocumentsAsync([FromRoute] Guid id, [FromForm] IFormFile file, [FromForm] string language)
        {
            var command = new UploadCompanyDocumentsCommand();

            if (file != null && file.Length > 0)
            {
                string fileName = file.FileName;
                int fileExtPos = fileName.LastIndexOf(".");
                if (fileExtPos >= 0)
                    fileName= fileName.Substring(0, fileExtPos);
                command.Title = fileName;
                command.Language = language;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    command.Content = stream.ToArray();
                }
            }
            else
            {
                throw new InvalidParameterException(nameof(command));
            }
            command.CompanyId = id;
            var response = await _mediator.Send(command);
            var res = new SuccessResult(response){
                StatusCode = StatusCodes.Status201Created
            };
            return new SuccessResult(response) { 
                StatusCode = StatusCodes.Status201Created 
            };
        }
    }
}
