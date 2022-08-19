using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Filters.FileValidation;
using Mosaico.API.Base.Responses;
using Mosaico.API.v1.DocumentManagement.Exceptions;
using Mosaico.Application.DocumentManagement.Commands.StoreFile;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.DeleteProjectDocument;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.ProjectPartnerProfile;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.ProjectTeamMemberProfile;
using Mosaico.Application.DocumentManagement.Commands.TokenPage.UpdateProjectDocument;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Application.DocumentManagement.Queries.GetProjectDocuments;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/projects/{id}/documents")]
    [Route("api/v{version:apiVersion}/projects/{id}/documents")]
    public partial class ProjectDocumentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProjectDocumentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPut("{documentId}")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]

        public async Task<IActionResult> UpdateProjectDocument([FromRoute] Guid id, [FromRoute] Guid documentId, [FromBody] ModifyDocumentDTO dto)
        {
            if (dto == null)
                throw new InvalidParameterException(nameof(dto));

            var command = _mapper.Map<UpdateProjectDocumentCommand>(dto);
            command.Id = documentId;
            command.ProjectId = id;

            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpDelete("{documentId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]

        public async Task<IActionResult> DeleteProjectDocument([FromRoute] Guid id, [FromRoute] Guid documentId)
        {
            var command = new DeleteProjectDocumentCommand { Id = documentId, ProjectId = id};
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpPost("files")]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        public async Task<IActionResult> StoreFile([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            var command = new ProjectTeamMemberProfileCommand
            {
                ProjectId = id
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

        [HttpPost("files/partner")]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> StoreFilePartner([FromRoute] Guid id, [FromForm] IFormFile file)
        {
            var command = new ProjectPartnerProfileCommand
            {
                ProjectId = id
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