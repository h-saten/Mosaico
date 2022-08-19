using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.UploadProjectDocuments;
using Mosaico.Application.ProjectManagement.Commands.UpsertProjectDocuments;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Queries.GetDocumentTypes;
using Mosaico.Application.ProjectManagement.Queries.GetProjectDocument;
using Mosaico.Application.ProjectManagement.Queries.GetProjectDocuments;
using Mosaico.Application.ProjectManagement.Queries.GetTemplateContent;
using Mosaico.Base;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        [HttpGet("documents/types")]
        [ProducesResponseType(typeof(SuccessResult<GetDocumentTypesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectDocumentsTypesAsync()
        {
            
            var response = await _mediator.Send(new GetDocumentTypesQuery
            {
            });
            return new SuccessResult(response);
        }

        [HttpGet("{id}/documents")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectDocumentsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProjectDocuments([FromRoute] Guid id, [FromQuery] string lang = null)
        {
            if (string.IsNullOrWhiteSpace(lang)) lang = _currentUserContext.Language;
            var response = await _mediator.Send(new GetProjectDocumentsQuery
            {
                ProjectId = id,
                Language = lang
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("templates/contents")]
        [ProducesResponseType(typeof(SuccessResult<GetTemplateContentQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetTemplateContentsAsync([FromQuery] GetTemplateContentQuery command)
        {

            if (command == null)
                throw new InvalidParameterException(nameof(command));

            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }

        [HttpGet("{id}/documents/{type}")]
        [ProducesResponseType(typeof(SuccessResult<DocumentContentDTO>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetDocumentContent([FromRoute] Guid id, [FromRoute] string type, [FromQuery] GetProjectDocumentQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.Language))
            {
                query.Language = Mosaico.Base.Constants.Languages.English;
            }
            query.ProjectId = id;
            query.Type = type;
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpPost("{id}/document")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpsertProjectDocumentsAsync([FromRoute] Guid id, [FromBody] UpsertProjectDocumentsCommand command)
        {
            if (command == null)
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.ProjectId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response){StatusCode = StatusCodes.Status201Created};
        }

        [HttpPost("{id}/document/upload")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue, MultipartHeadersLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadProjectDocumentsAsync([FromRoute] Guid id, [FromForm] IFormFile file, [FromForm] string language, [FromForm] string type)
        {
            var command = new UploadProjectDocumentsCommand();

            if (file != null && file.Length > 0)
            {
                command.Language = language;
                command.Type = type;
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
    }
}