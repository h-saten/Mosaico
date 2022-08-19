using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.UploadProjectLogo;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/projects/{id}/logo")]
    [Route("api/v{version:apiVersion}/projects/{id}/logo")]
    public partial class ProjectLogoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectLogoController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UploadProjectLogo([FromRoute] string id, [FromForm] IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
            {
                throw new InvalidParameterException(nameof(id));
            }
            var command = new UploadProjectLogoCommand
            {
                ProjectId = projectId
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