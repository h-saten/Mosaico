using System;
using System.IO;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.User.DeleteUserPhoto;
using Mosaico.Application.DocumentManagement.Commands.User.UploadUserPhoto;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/users/{id}/photo")]
    [Route("api/v{version:apiVersion}/users/{id}/photo")]
    [AllowAnonymous]
    public class UserPhotoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserPhotoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpDelete]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> DeleteUserPhoto([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var userId))
            {
                throw new InvalidParameterException(nameof(id));
            }

            var command = new DeleteUserPhotoCommand
            {
                UserId = userId
            };
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadUserPhoto([FromRoute] string id, [FromForm] IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var userId))
            {
                throw new InvalidParameterException(nameof(id));
            }
            var command = new UploadUserPhotoCommand
            {
                UserId = userId
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