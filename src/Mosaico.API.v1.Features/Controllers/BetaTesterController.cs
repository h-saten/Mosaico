using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Features.Commands.AddBetaTester;
using Mosaico.Application.Features.Commands.UpdateBetaTester;

namespace Mosaico.API.v1.Features.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/beta/testers")]
    [Route("api/v{version:apiVersion}/beta/testers")]
    public class BetaTesterController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BetaTesterController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status409Conflict)]
        [Authorize]
        public async Task<IActionResult> CreateBetaTester([FromBody] AddBetaTesterCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));

            var response = await _mediator.Send(command);
            return new SuccessResult(response) {StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status409Conflict)]
        [Authorize]
        public async Task<IActionResult> UpdateBetaTester([FromRoute] Guid id, [FromBody] UpdateBetaTesterCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Id = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}