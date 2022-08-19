using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadArticleCover;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadArticlePhoto;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/pages/{id}/article")]
    [Route("api/v{version:apiVersion}/pages/{id}/article")]
    public class ArticleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticleController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost("cover")]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UploadArticleCover([FromRoute] Guid id, [FromForm] IFormFile file, [FromQuery] string language =  Mosaico.Base.Constants.Languages.English)
        {
            var command = new UploadArticleCoverCommand
            {
                ArticleId = id,
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

            var result = await _mediator.Send(command);

            return new SuccessResult(result) { StatusCode = 201 };
        }

        [HttpPost("photo")]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UploadArticlePhoto([FromRoute] Guid id, [FromForm] IFormFile file, [FromQuery] string language = Mosaico.Base.Constants.Languages.English)
        {
            var command = new UploadArticlePhotoCommand
            {
                ArticleId = id,
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

            var result = await _mediator.Send(command);

            return new SuccessResult(result) { StatusCode = 201 };
        }
    }
}