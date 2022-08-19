using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.Staking.Claim;
using Mosaico.Application.Wallet.Commands.Staking.Distribute;
using Mosaico.Application.Wallet.Commands.Staking.Stake;
using Mosaico.Application.Wallet.Commands.Staking.StakeMetamask;
using Mosaico.Application.Wallet.Commands.Staking.UpdateStakingRegulation;
using Mosaico.Application.Wallet.Commands.Staking.Withdraw;
using Mosaico.Application.Wallet.Commands.Staking.WithdrawMetamask;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.Staking.GetRewardEstimate;
using Mosaico.Application.Wallet.Queries.Staking.GetStakingPairs;
using Mosaico.Application.Wallet.Queries.Staking.GetStakings;
using Mosaico.Application.Wallet.Queries.Staking.GetStakingStatistics;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/staking")]
    [Route("api/v{version:apiVersion}/staking")]
    [Authorize]
    public class StakingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUserContext;

        public StakingController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }

        [HttpGet("pairs")]
        [ProducesResponseType(typeof(SuccessResult<List<StakingPairDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStakingPairs()
        {
            var response = await _mediator.Send(new GetStakingPairsQuery());
            return new  SuccessResult(response);
        }

        [HttpPost("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Stake([FromRoute] Guid id, [FromBody] StakeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.StakingPairId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{id}/metamask")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> StakeMetamask([FromRoute] Guid id, [FromBody] StakeMetamaskCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.StakingPairId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{id}/metamask/withdrawal")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> WithdrawMetamask([FromRoute] Guid id, [FromBody] WithdrawMetamaskCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.Id = id;
            command.UserId = _currentUserContext.UserId;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{id}/withdrawal")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Withdraw([FromRoute] Guid id)
        {
            await _mediator.Send(new WithdrawCommand
            {
                Id = id,
                UserId = _currentUserContext.UserId
            });
            return new SuccessResult();
        }

        [HttpPost("{id}/reward")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Claim([FromRoute] Guid id)
        {
            var command = new ClaimCommand
            {
                UserId = _currentUserContext.UserId,
                Id = id
            };
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpGet("{id}/estimate")]
        [ProducesResponseType(typeof(SuccessResult<GetRewardEstimateQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRewardEstimate([FromRoute] Guid id)
        {
            var query = new GetRewardEstimateQuery
            {
                UserId = _currentUserContext.UserId,
                Id = id
            };
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
        
        [HttpPost("{id}/distribution")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Distribute([FromRoute] Guid id, [FromBody] DistributeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = _currentUserContext.UserId;
            command.StakingPairId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetStakingsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStakings()
        {
            var response = await _mediator.Send(new GetStakingsQuery
            {
                UserId = _currentUserContext.UserId
            });
            return new  SuccessResult(response);
        }
        
        [HttpGet("statistics")]
        [ProducesResponseType(typeof(SuccessResult<GetStakingStatisticsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStakingStatistics()
        {
            var response = await _mediator.Send(new GetStakingStatisticsQuery
            {
                UserId = _currentUserContext.UserId
            });
            return new  SuccessResult(response);
        }

        [HttpPost("{id}/regulations")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateRegulations([FromRoute] Guid id, [FromBody] UpdateStakingRegulationCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.StakingPairId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}