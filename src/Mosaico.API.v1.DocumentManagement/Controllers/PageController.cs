using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.DeletePageCover;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadPageCover;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/pages/{id}/cover")]
    [Route("api/v{version:apiVersion}/pages/{id}/cover")]
    public class PageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PageController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UploadProjectCover([FromRoute] Guid id, [FromForm] IFormFile file, [FromQuery] string language =  Mosaico.Base.Constants.Languages.English)
        {
            var command = new UploadPageCoverCommand
            {
                PageId = id,
                Language = language
            };
            if (file != null && file.Length > 0)
            {
                command.FileName = file.FileName;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    command.Content = stream.ToArray();
                }
            }

            var fileId = await _mediator.Send(command);

            return new SuccessResult(fileId) { StatusCode = 201 };
        }

        [HttpDelete]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> DeleteProjectCover([FromRoute] Guid id, [FromQuery] string language = Mosaico.Base.Constants.Languages.English)
        {
            await _mediator.Send(new DeletePageCoverCommand
            {
                Language = language,
                PageId = id
            });
            return new SuccessResult();
        }
    }
}