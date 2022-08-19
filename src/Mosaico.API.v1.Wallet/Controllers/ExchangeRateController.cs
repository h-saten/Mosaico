using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Queries.GetExchangeRates;
using Mosaico.Application.Wallet.Queries.GetHistoricalExchangeRate;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/rates")]
    [Route("api/v{version:apiVersion}/rates")]
    [AllowAnonymous]
    public class ExchangeRateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExchangeRateController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetExchangeRatesQueryResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetRates()
        {
            var response = await _mediator.Send(new GetExchangeRatesQuery());
            return new SuccessResult(response);
        }
        
        [HttpGet("history")]
        [ProducesResponseType(typeof(SuccessResult<GetHistoricalExchangeRateQueryResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetHistoricalRate([FromQuery] GetHistoricalExchangeRateQuery query)
        {
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
    }
}