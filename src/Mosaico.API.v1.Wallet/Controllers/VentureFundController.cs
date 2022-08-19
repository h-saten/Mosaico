using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Queries.VentureFund.GetVentureFundHistory;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/fund")]
    [Route("api/v{version:apiVersion}/fund")]
    [AllowAnonymous]
    public class VentureFundController : ControllerBase
    {
        private readonly IMediator _mediator;

        public VentureFundController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetVentureFundHistoryQueryResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetVentureFundHistory([FromQuery] GetVentureFundHistoryQuery query)
        {
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
    }
}