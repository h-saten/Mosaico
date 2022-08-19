using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.DocumentManagement.Commands.DAO.CompanyDocumentFile;
using Mosaico.Application.DocumentManagement.Commands.DAO.CreateCompanyDocument;
using Mosaico.Application.DocumentManagement.Commands.DAO.DeleteCompanyDocument;
using Mosaico.Application.DocumentManagement.Commands.DAO.UpdateCompanyDocument;
using Mosaico.Application.DocumentManagement.DTOs;
using Mosaico.Application.DocumentManagement.Queries.GetProjectDocuments;
using Mosaico.Application.DocumentManagement.Queries.GetCompanyDocuments;

namespace Mosaico.API.v1.DocumentManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/dao/{id}/documents")]
    [Route("api/v{version:apiVersion}/dao/{id}/documents")]
    public partial class CompanyDocumentController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public CompanyDocumentController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetCompanyDocumentQuery>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCompanyDocuments([FromRoute] Guid id, [FromQuery] string language)
        {
            var query = new GetCompanyDocumentQuery { CompanyId = id, Language = language };
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]

        public async Task<IActionResult> CreateCompanyDocument([FromRoute] Guid id, [FromBody] ModifyDocumentDTO dto)
        {
            if (dto == null)
                throw new InvalidParameterException(nameof(dto));

            var command = _mapper.Map<CreateCompanyDocumentCommand>(dto);
            command.CompanyId = id;

            var docuemntId = await _mediator.Send(command);
            return new SuccessResult(docuemntId) { StatusCode = StatusCodes.Status201Created };
        }

        [HttpPut("{documentId}")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]

        public async Task<IActionResult> UpdateCompanyDocument([FromRoute] Guid documentId, [FromBody] ModifyDocumentDTO dto)
        {
            if (dto == null)
                throw new InvalidParameterException(nameof(dto));

            var command = _mapper.Map<UpdateCompanyDocumentCommand>(dto);
            command.Id = documentId;

            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpDelete("{documentId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]

        public async Task<IActionResult> DeleteCompanyDocument([FromRoute] Guid documentId)
        {
            var command = new DeleteCompanyDocumentCommand() { Id = documentId };
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpPost("files")]
        [ProducesResponseType(typeof(SuccessResult<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> StoreFile([FromForm] IFormFile file)
        {
            var command = new CompanyDocumentFileCommand();
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