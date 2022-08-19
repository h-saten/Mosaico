using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.Crowdsale.DeployStage;
using Mosaico.Application.ProjectManagement.Commands.DeleteStage;
using Mosaico.Application.ProjectManagement.Commands.SetProjectStagePurchaseLimit;
using Mosaico.Application.ProjectManagement.Commands.SubscribePrivateSale;
using Mosaico.Application.ProjectManagement.Commands.UpdateProjectStages;
using Mosaico.Application.ProjectManagement.Commands.UpsertProjectStage;
using Mosaico.Application.ProjectManagement.Queries.GetProjectStage;
using Mosaico.Application.ProjectManagement.Queries.GetProjectStages;
using Mosaico.Application.ProjectManagement.Queries.GetStageAuthorizationCode;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        [HttpPost("{id}/stages")]
        [ProducesResponseType(typeof(SuccessResult<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> UpdateProjectStages([FromRoute] Guid id, [FromBody] UpdateProjectStagesCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Id = id;
            var response = await _mediator.Send(command);
        
            return new SuccessResult(response);
        }
        
        [HttpGet("{id}/stages")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectsStagesQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectStages([FromRoute] Guid id)
        {
            var stages = await _mediator.Send(new GetProjectStagesQuery
            {
                Id = id,
            });

            return new SuccessResult(stages);
        }
        
        [HttpGet("{id}/stages/{stageId}")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectStageQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProjectStage([FromRoute] Guid id, [FromRoute] Guid stageId)
        {
            var stage = await _mediator.Send(new GetProjectStageQuery
            {
                ProjectId = id,
                StageId = stageId
            });

            return new SuccessResult(stage);
        }
        
        [HttpDelete("{id}/stages/{stageId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProjectStage([FromRoute] Guid id, [FromRoute] Guid stageId)
        {
            await _mediator.Send(new DeleteStageCommand
            {
                ProjectId = id,
                StageId = stageId
            });

            return new SuccessResult();
        }
        
        // [HttpPost("{id}/stages")]
        // [ProducesResponseType(typeof(SuccessResult<GetProjectStageQueryResponse>), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status409Conflict)]
        // public async Task<IActionResult> CreateProjectStage([FromRoute] Guid id, [FromBody] UpsertProjectStageCommand command)
        // {
        //     if (command == null) throw new InvalidParameterException(nameof(command));
        //     command.ProjectId = id;
        //     await _mediator.Send(command);
        //     return new SuccessResult();
        // }
        //
        
        [HttpPut("{id}/stages/{stageId?}")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectStageQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProjectStage([FromRoute] Guid id, [FromBody] UpsertProjectStageCommand command, [FromRoute] Guid? stageId = null)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = id;
            command.StageId = stageId;
            var updatedStageId = await _mediator.Send(command);
            return new SuccessResult(updatedStageId);
        }
        
        [HttpPost("{id}/stages/{stageId}/contract")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeployStage([FromRoute] Guid id, [FromRoute] Guid stageId)
        {
            await _mediator.Send(new DeployStageCommand
            {
                ProjectId = id,
                StageId = stageId
            });
            return new SuccessResult();
        }
        
        [HttpPut("stages")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> SubscribeToSale([FromBody] string code)
        {
            var response = await _mediator.Send(new SubscribePrivateSaleCommand
            {
                AuthorizationCode = code
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{id}/stages/{stageId}/code")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetAuthorizationCode([FromRoute] Guid id, [FromRoute] Guid stageId)
        {
            var response = await _mediator.Send(new GetStageAuthorizationCodeQuery
            {
                ProjectId = id,
                StageId = stageId
            });
            return new SuccessResult(response);
        }

        [HttpPost("{id}/stages/{stageId}/limit")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> SetStagePurchaseLimit([FromRoute] Guid id, [FromRoute] Guid stageId, [FromBody] SetProjectStagePurchaseLimitCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = id;
            command.StageId = stageId;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}