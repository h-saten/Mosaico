using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.SetProjectPaymentDetails;
using Mosaico.Application.Wallet.Commands.Transactions.UpdateProjectFee;
using Mosaico.Application.Wallet.Queries.GetPaymentDetails;
using Mosaico.Application.Wallet.Queries.GetProjectWallet;
using Mosaico.Application.Wallet.Queries.Project.GetProjectFee;
using Mosaico.Application.Wallet.Queries.Project.GetProjectInvestor;
using Mosaico.Application.Wallet.Queries.TransactionFee;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/projects")]
    [Route("api/v{version:apiVersion}/projects")]
    [ApiController]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/investors/{userId}")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectInvestorQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetProjectInvestors([FromRoute] Guid id, [FromRoute] string userId)
        {
            var response = await _mediator.Send(new GetProjectInvestorQuery
            {
                ProjectId = id,
                UserId = userId
            });
            return new SuccessResult(response);
        }

        [HttpGet("{id}/wallet")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectWalletQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetProjectWallet([FromRoute] Guid id, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            if (skip < 0)
            {
                skip = 0;
            }

            if (take <= 0)
            {
                take = 10;
            }
            
            var response = await _mediator.Send(new GetProjectWalletQuery
            {
                ProjectId = id,
                Skip = skip,
                Take = take
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("{id}/bank")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpsertBankPaymentDetails([FromRoute] Guid id, [FromBody] SetProjectPaymentDetailsCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpGet("{id}/bank")]
        [ProducesResponseType(typeof(SuccessResult<GetPaymentDetailsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetPaymentDetails([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetPaymentDetailsQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }

        [HttpGet("{id}/fee")]
        [ProducesResponseType(typeof(SuccessResult<TransactionFeeQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetTransactionFeesForMosaico([FromRoute] Guid id, [FromQuery] string type = "MOSAICO")
        {
            var response = await _mediator.Send(new TransactionFeeQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("{id}/settings/fee")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateTransactionFee([FromRoute] Guid id, [FromBody] UpdateProjectFeeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
        
        [HttpGet("{id}/settings/fee")]
        [ProducesResponseType(typeof(SuccessResult<decimal>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetProjectFeePercentage([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetProjectFeeQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }

    }
}