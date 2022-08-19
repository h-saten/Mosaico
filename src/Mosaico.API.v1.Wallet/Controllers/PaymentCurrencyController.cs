using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.GetPaymentCurrencies;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/currencies")]
    [Route("api/v{version:apiVersion}/currencies")]
    [ApiController]
    public class PaymentCurrencyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentCurrencyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<List<PaymentCurrencyDTO>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetPaymentCurrency([FromQuery] string network = Blockchain.Base.Constants.BlockchainNetworks.Default)
        {
            var response = await _mediator.Send(new GetPaymentCurrenciesQuery
            {
                Network = network
            });
            return new SuccessResult(response);
        }
    }
}