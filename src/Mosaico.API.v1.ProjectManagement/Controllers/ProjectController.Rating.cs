using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.Rating.Like;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        [HttpPost("{id}/like")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> Like([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new LikeProjectCommand
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
    }
}