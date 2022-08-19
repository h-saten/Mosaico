
namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        // [HttpGet("{id}/staking")]
        // public async Task<IActionResult> GetStakingAsync([FromRoute] string id)
        // {
        //     if (string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var projectId))
        //         throw new InvalidParameterException(nameof(id));
        //     
        //     var response = await _mediator.Send(new GetStakingQuery
        //     {
        //         ProjectId = projectId
        //     });
        //     return new SuccessResult(response);
        // }

        // [HttpPost("{id}/staking")]
        // public async Task<IActionResult> CreateStakingAsync([FromRoute] string id, [FromBody] UpsertProjectStakingCommand command)
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