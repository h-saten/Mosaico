using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.Token.UploadTokenLogo;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/tokens/{id}/logo")]
    [Route("api/v{version:apiVersion}/tokens/{id}/logo")]
    public class TokenLogoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TokenLogoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UploadTokenLogo([FromRoute] string id, [FromForm] IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var tokenId))
            {
                throw new InvalidParameterException(nameof(id));
            }
            var command = new UploadTokenLogoCommand
            {
                TokenId = tokenId
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
    }
}