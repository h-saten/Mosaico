using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.Administration.ApproveProject;
using Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectVisibility;
using Mosaico.Application.ProjectManagement.Queries.Administration.GetAdminProjects;
using Mosaico.Application.ProjectManagement.Queries.GetProjects;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/admin/projects")]
    [Route("api/v{version:apiVersion}/admin/projects")]
    [Authorize]
    public class ProjectAdministrationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectAdministrationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetProjectsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProjects([FromQuery] GetAdminProjectsQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpPut("{id}/status/acceptance")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptProject([FromRoute] Guid id)
        {
            await _mediator.Send(new ApproveProjectCommand
            {
                ProjectId = id
            });

            return new SuccessResult();
        }
        
        [HttpPut("{id}/visibility")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateVisibilityProject([FromRoute] Guid id, UpdateProjectVisibilityCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Id = id;

            await _mediator.Send(command);

            return new SuccessResult();
        }
    }
}