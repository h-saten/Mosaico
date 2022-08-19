using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.UploadCertificateBackground;
using Mosaico.Application.ProjectManagement.Commands.UpsertCertificateConfiguration;
using Mosaico.Application.ProjectManagement.Queries.GetCertificateConfiguration;
using Mosaico.Application.ProjectManagement.Queries.GetDocumentTypes;
using Mosaico.Application.ProjectManagement.Queries.GetExampleCertificate;
using Mosaico.Application.ProjectManagement.Queries.GetUserCertificate;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        
        [HttpGet("{id:guid}/certificate/configuration")]
        [ProducesResponseType(typeof(SuccessResult<GetDocumentTypesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetConfigurationAsync([FromRoute] Guid id, [FromQuery] string language)
        {
            var response = await _mediator.Send(new GetCertificateConfigurationQuery
            {
                Language = language,
                ProjectId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("{id:guid}/certificate/configuration")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpsertCertificateConfigurationAsync([FromRoute] Guid id, [FromBody] UpsertCertificateConfigurationCommand command)
        {
            if (command == null)
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.ProjectId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = StatusCodes.Status201Created};
        }

        [HttpPost("{id:guid}/certificate/background")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UploadCertificateBackgroundAsync([FromRoute] Guid id, [FromForm] IFormFile file, [FromForm] string language, [FromForm] string type)
        {

            var command = new UploadCertificateBackgroundCommand();

            if (file != null && file.Length > 0)
            {
                command.Language = language;
                command.OriginalFileName = file.FileName;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    command.Content = stream.ToArray();
                }
            } else
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.ProjectId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpGet("{id:guid}/certificate/example")]
        [ProducesResponseType(typeof(SuccessResult<GetDocumentTypesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExampleCertificate([FromRoute] Guid id, [FromQuery] string language)
        {
            var response = await _mediator.Send(new GetExampleCertificateQuery
            {
                Language = language,
                ProjectId = id
            });
            return new SuccessResult(response);
        }

        [HttpGet("{id:guid}/certificate")]
        [ProducesResponseType(typeof(SuccessResult<GetDocumentTypesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UserCertificate([FromRoute] Guid id, [FromQuery] string language)
        {
            var response = await _mediator.Send(new GetUserCertificateQuery
            {
                Language = language,
                ProjectId = id,
                UserId = _currentUserContext.UserId
            });
            return new SuccessResult(response);
        }
    }
}