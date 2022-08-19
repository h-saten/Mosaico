using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletCurrencySend;
using Mosaico.Application.Wallet.Commands.Wallet.CompanyWalletTokenSend;
using Mosaico.Application.Wallet.Queries.Company.CompanyTokenHolders;
using Mosaico.Application.Wallet.Queries.Company.CompanyWalletTokens;
using Mosaico.Application.Wallet.Queries.Company.GetCompanyPaymentCurrencyBalance;
using Mosaico.Application.Wallet.Queries.GetCompanyPaymentDetails;
using Mosaico.Application.Wallet.Services.Abstractions;
using Mosaico.Domain.Wallet.Abstractions;
using Newtonsoft.Json;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/dao")]
    [Route("api/v{version:apiVersion}/dao")]
    [Authorize]
    public class CompanyWalletController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public CompanyWalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/wallet")]
        [ProducesResponseType(typeof(SuccessResult<CompanyWalletTokensQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCompanyWalletTokens([FromRoute] Guid id, [FromQuery] string token = null)
        {
            var response = await _mediator.Send(new CompanyWalletTokensQuery
            {
                CompanyId = id,
                TokenTicker = token
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("{id}/transaction")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TransferTokens([FromRoute] Guid id, [FromBody] CompanyWalletSendTokenCommand command)
        {
            if (command == null)
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.CompanyId = id;
            
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPost("{id}/currency/transaction")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> TransferCurrency([FromRoute] Guid id, [FromBody] CompanyWalletSendCurrencyCommand command)
        {
            if (command == null)
            {
                throw new InvalidParameterException(nameof(command));
            }

            command.CompanyId = id;
            
            await _mediator.Send(command);
            return new SuccessResult();
        }

        [HttpGet("{id}/payment-details")]
        [ProducesResponseType(typeof(SuccessResult<GetCompanyPaymentDetailsQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaymentDetails([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetCompanyPaymentDetailsQuery
            {
                Currency = "USDT",
                CompanyId = id
            });
            return new SuccessResult(response);
        }

        [HttpGet("{id}/holders")]
        [ProducesResponseType(typeof(SuccessResult<CompanyTokenHoldersQueryResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CompanyHolders([FromRoute] Guid id, [FromQuery] CompanyTokenHoldersQuery query)
        {
            if (query == null) throw new InvalidParameterException(nameof(query));
            query.CompanyId = id;
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }

        [HttpGet("{companyId}/balance")]
        [ProducesResponseType(typeof(SuccessResult<GetCompanyPaymentCurrencyBalanceQuery>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetNativeBalance([FromRoute] Guid companyId)
        {
            var response = await _mediator.Send(new GetCompanyPaymentCurrencyBalanceQuery
            {
                CompanyId = companyId
            });
            return new SuccessResult(response);
        }
    }
}