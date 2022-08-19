using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.AcceptProjectInvitation;
using Mosaico.Application.ProjectManagement.Commands.AddProjectMember;
using Mosaico.Application.ProjectManagement.Commands.DeleteProjectMember;
using Mosaico.Application.ProjectManagement.Commands.UpdateProjectMemberRole;
using Mosaico.Application.ProjectManagement.Queries.GetProjectMembers;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        [HttpPost("{id}/members")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> AddProjectMember([FromRoute] string id, [FromBody] AddProjectMemberCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
                throw new InvalidParameterException(nameof(id));
            command.ProjectId = projectId;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpDelete("{id}/members/{memberId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DeleteProjectMember([FromRoute] string id, [FromRoute] string memberId)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(memberId, out var memberGuid))
                throw new InvalidParameterException(nameof(memberId));
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
                throw new InvalidParameterException(nameof(id));
            await _mediator.Send(new DeleteProjectMemberCommand
            {
                ProjectMemberId = memberGuid,
                ProjectId = projectId
            });
            return new SuccessResult();
        }

        [HttpPut("{id}/members/{memberId}")]
        public async Task<IActionResult> UpdateProjectMemberRole([FromRoute] string id, [FromRoute] string memberId, [FromBody] UpdateProjectMemberCommand command)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(memberId, out var memberGuid))
                throw new InvalidParameterException(nameof(memberId));
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
                throw new InvalidParameterException(nameof(id));
            if (command == null)
                throw new InvalidParameterException(nameof(command));
            command.ProjectId = projectId;
            command.MemberId = memberGuid;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        

        [HttpPost("invitations")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> AcceptMemberInvitation([FromBody] AcceptProjectInvitationCommand command)
        {
            if (command == null)
                throw new InvalidParameterException("code");
            
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }

        [HttpGet("{id}/members")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectMembersQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetProjectMembers([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
                throw new InvalidParameterException(nameof(id));
            var response = await _mediator.Send(new GetProjectMembersQuery
            {
                ProjectId = projectId
            });
            return new SuccessResult(response);
        }
    }
}