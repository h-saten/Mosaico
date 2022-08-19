using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.Vesting.CreateVesting;
using Mosaico.Application.Wallet.Commands.Vesting.RedeployVesting;
using Mosaico.Application.Wallet.Queries.Vesting.GetPersonalVestings;
using Mosaico.Authorization.Base;
using Mosaico.SDK.Base.Models;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/vesting")]
    [Route("api/v{version:apiVersion}/vesting")]
    [Authorize]
    public class VestingController : ControllerBase
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMediator _mediator;

        public VestingController(ICurrentUserContext currentUserContext, IMediator mediator)
        {
            _currentUserContext = currentUserContext;
            _mediator = mediator;
        }

        [HttpPut]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> CreateVesting([FromBody] CreateVestingCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            var response = await _mediator.Send(command);
            return new SuccessResult(response) {StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpPost("{id}/contract")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> RedeployVesting([FromRoute] Guid id)
        {
            await _mediator.Send(new RedeployVestingCommand
            {
                VestingId = id
            });
            return new SuccessResult();
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetPersonalVestingsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVesting([FromQuery] Guid tokenId, [FromQuery] string type)
        {
            switch (type)
            {
                case "PERSONAL":
                {
                    var response = await _mediator.Send(new GetPersonalVestingsQuery
                    {
                        TokenId = tokenId
                    });
                    return new SuccessResult(response);
                }
                default: throw new InvalidParameterException(nameof(type));
            }
        }
    }
}