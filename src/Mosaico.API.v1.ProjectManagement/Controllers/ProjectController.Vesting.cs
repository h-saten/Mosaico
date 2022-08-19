namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        // [HttpGet("{id}/vesting")]
        // [ProducesResponseType(typeof(SuccessResult<GetVestingQueryResponse>), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        // public async Task<IActionResult> GetVestingAsync([FromRoute] string id)
        // {
        //     if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
        //         throw new InvalidParameterException(nameof(id));
        //     
        //     var response = await _mediator.Send(new GetVestingQuery
        //     {
        //         ProjectId = projectId
        //     });
        //     return new SuccessResult(response);
        // }

        // [HttpPost("{id}/vesting")]
        // [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        // [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        // [Authorize]
        // public async Task<IActionResult> CreateVestingAsync([FromRoute] string id, [FromBody] UpsertProjectVestingCommand command)
        // {
        //     if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
        //         throw new InvalidParameterException(nameof(id));
        //     if (command == null)
        //     {
        //         throw new InvalidParameterException(nameof(command));
        //     }
        //
        //     command.ProjectId = projectId;
        //     var response = await _mediator.Send(command);
        //     return new SuccessResult(response){StatusCode = StatusCodes.Status201Created};
        // }
    }
}