using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.Administration.UpdateProjectPublicity;
using Mosaico.Application.ProjectManagement.Commands.CreateProject;
using Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployCrowdsale;
using Mosaico.Application.ProjectManagement.Commands.SubmitProjectForReview;
using Mosaico.Application.ProjectManagement.Commands.SubscribePrivateSale;
using Mosaico.Application.ProjectManagement.Commands.UpdateProject;
using Mosaico.Application.ProjectManagement.Commands.UpdateProjectVisitor;
using Mosaico.Application.ProjectManagement.DTOs;
using Mosaico.Application.ProjectManagement.Permissions;
using Mosaico.Application.ProjectManagement.Queries.GetCompanyProjects;
using Mosaico.Application.ProjectManagement.Queries.GetProject;
using Mosaico.Application.ProjectManagement.Queries.GetProjectForUpdate;
using Mosaico.Application.ProjectManagement.Queries.GetProjects;
using Mosaico.Application.ProjectManagement.Queries.GetProjectVisitors;
using Mosaico.Application.ProjectManagement.Queries.GetTokenomics;
using Mosaico.Application.ProjectManagement.Queries.GetUserProjects;
using Mosaico.Application.ProjectManagement.Queries.ProjectPermission;
using Mosaico.Application.ProjectManagement.Queries.ProjectPreValidation;
using Mosaico.Application.ProjectManagement.Queries.ProjectScore;
using Mosaico.Authorization.Base;
using Mosaico.Base;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("api/projects")]
    [Route("api/v{version:apiVersion}/projects")]
    public partial class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUserContext;
        
        public ProjectController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetProjectsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProjects([FromQuery] GetProjectsQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
        
        [HttpGet("/api/dao/{id}/projects")]
        [ProducesResponseType(typeof(SuccessResult<PaginatedResult<ProjectDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetCompanyProjects([FromRoute] Guid id, [FromQuery] GetCompanyProjectsQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            query.CompanyId = id;
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
        
        [HttpGet("/api/self/projects")]
        [ProducesResponseType(typeof(SuccessResult<PaginatedResult<ProjectDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetUserProjects([FromQuery] GetUserProjectsQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.UserId))
            {
                query.UserId = _currentUserContext.UserId;
            }
            
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProject([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidParameterException(nameof(id));
            var response = await _mediator.Send(new GetProjectQuery {UniqueIdentifier = id});
            return new SuccessResult(response);
        }

        /*
         * <summary>
         *  Create project
         * </summary>
         * <param name="dto">Parameter of type CreateProjectCommand with TokenName, Title and Description</param>
         * <returns>ID of the new user (GUID)</returns>
         */
        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Title = command.Title.Trim();
            var id = await _mediator.Send(command);
            return new SuccessResult(id) { StatusCode = 201 };
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateProject([FromRoute] string id, [FromBody] UpdateProjectCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
                throw new InvalidParameterException(nameof(id));

            command.ProjectId = projectId;
            await _mediator.Send(command);

            return new SuccessResult();
        }
        
        [HttpPut("{id}/status/review")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> SubmitProjectForReview([FromRoute] Guid id)
        {
            await _mediator.Send(new SubmitProjectForReviewCommand
            {
                ProjectId = id
            });

            return new SuccessResult();
        }

        [HttpGet("{id}/permissions")]
        [ProducesResponseType(typeof(SuccessResult<ProjectPermissions>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserPermissions([FromRoute] string id, [FromQuery] string userId = null)
        {
            if (string.IsNullOrWhiteSpace(id)) throw new InvalidParameterException(nameof(id));

            if (string.IsNullOrWhiteSpace(userId) && _currentUserContext.IsAuthenticated)
            {
                userId = _currentUserContext.UserId;
            }

            var response = await _mediator.Send(new ProjectPermissionQuery()
            {
                UniqueIdentifier = id,
                UserId = userId
            });

            return new SuccessResult(response);
        }

        [HttpPost("pre-validation")]
        [ProducesResponseType(typeof(SuccessResult<ProjectPreValidationQueryResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetProjectPreValidation([FromBody] ProjectPreValidationQuery query)
        {
            if (query == null)
            {
                throw new InvalidParameterException(nameof(query));
            }

            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpPatch("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> PatchProject([FromRoute] Guid id, [FromBody] JsonPatchDocument<UpdateProjectDTO> doc)
        {
            if (doc == null) throw new InvalidParameterException(nameof(doc));
            var projectDTO = await _mediator.Send(new GetProjectForUpdateQuery {Id = id});
            doc.ApplyTo(projectDTO);
            
            await _mediator.Send(new UpdateProjectCommand
            {
                ProjectId = id,
                Project = projectDTO
            });
            return new SuccessResult();
        }

        [HttpGet("{id}/score")]
        [ProducesResponseType(typeof(SuccessResult<ProjectScoreQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetProjectScore([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new ProjectScoreQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }

        [HttpGet("{id}/tokenomics")]
        [ProducesResponseType(typeof(SuccessResult<GetTokenomicsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProjectTokenomics([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetTokenomicsQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }

        [HttpPost("{id}/crowdsale")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeployCrowdsaleAsync([FromRoute] Guid id)
        {
            await _mediator.Send(new DeployCrowdsaleCommand
            {
                ProjectId = id
            });
            return new SuccessResult();
        }

        [HttpPost("{id}/updatevisitor")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateVisitorAsync([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new UpdateProjectVisitorCommand
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }

        [HttpGet("{id}/isprojectvisited")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> CheckIsProjectVisited([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetProjectVisitorsQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response.isProjectVisited);
        }
        
        [HttpPut("{id}/publicity")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProjectPublicity([FromRoute] Guid id, UpdateProjectPublicityCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}