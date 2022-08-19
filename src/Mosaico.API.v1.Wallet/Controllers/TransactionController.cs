using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Filters;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.Transactions.ConfirmDepositTransaction;
using Mosaico.Application.Wallet.Commands.Transactions.ExportTransactions;
using Mosaico.Application.Wallet.Commands.Transactions.UpdateSalesAgent;
using Mosaico.Application.Wallet.Commands.Transactions.UpdateTransaction;
using Mosaico.Application.Wallet.Commands.Transactions.UpdateTransactionFee;
using Mosaico.Application.Wallet.Queries.Company.CompanyWalletTransactions;
using Mosaico.Application.Wallet.Queries.GetProjectTransactions;
using Mosaico.Application.Wallet.Queries.Operations.GetTransactionOperations;
using Mosaico.Application.Wallet.Queries.WalletTransactions;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/transactions")]
    [Route("api/v{version:apiVersion}/transactions")]
    [Authorize]
    public class TransactionController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public TransactionController(IMediator mediator)
        {
            _mediator = mediator;
        }
        

        [HttpPut("{transactionId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmDepositTransaction([FromRoute] string transactionId, [FromBody] ConfirmDepositTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            if (!Guid.TryParse(transactionId, out var transactionGuid))
                throw new InvalidParameterException(nameof(transactionGuid));
            command.TransactionId = transactionGuid;
            await _mediator.Send(command);
            return new SuccessResult(null);
        }
        
        [HttpGet("{wallet}/{network}")]
        [ProducesResponseType(typeof(SuccessResult<WalletTransactionsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTransactionsAsync([FromRoute] string wallet, [FromRoute] string network, [FromQuery] int skip = 0, [FromQuery] int take = 30)
        {
            var response = await _mediator.Send(new WalletTransactionsQuery
            {
                Network = network,
                WalletAddress = wallet,
                Skip = skip,
                Take = take
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("dao/{id}")]
        [ProducesResponseType(typeof(SuccessResult<CompanyWalletTransactionsResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompanyWalletTransactionsAsync([FromRoute] Guid id, [FromQuery] int skip = 0, [FromQuery] int take = 30)
        {
            var response = await _mediator.Send(new CompanyWalletTransactionsQuery
            {
                CompanyId = id,
                Skip = skip,
                Take = take
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("project")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectTransactionsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProjectTransactions([FromQuery] GetProjectTransactionsQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            var user = await _mediator.Send(query);
            return new SuccessResult(user);
        }
        
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateTransaction([FromRoute] Guid id, [FromBody] UpdateTransactionCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TransactionId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpPost("export")]
        [Authorize]
        public async Task<IActionResult> ExportTransaction([FromQuery] ExportTransactionsCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            var response = await _mediator.Send(command);
            Response.Headers.Add("filename", response.Filename);
            var file = File(response.File, response.ContentType, response.Filename);
            return file;
        }
        
        [HttpGet("{id}/operations")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResult<GetTransactionOperationsQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetTransactionOperations([FromRoute] Guid id, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            if (skip < 0) skip = 0;
            if (take < 0 || take > 100) take = 10;
            var response = await _mediator.Send(new GetTransactionOperationsQuery
            {
                Skip = skip,
                Take = take,
                TransactionId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpPut("{id}/agent")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateSalesAgent([FromRoute] Guid id, [FromBody] UpdateSalesAgentCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TransactionId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPut("{id}/fee")]
        [Authorize]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateFee([FromRoute] Guid id, [FromBody] UpdateTransactionFeeCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.TransactionId = id;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}