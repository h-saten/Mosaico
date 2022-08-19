using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.GetExternalExchanges;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/exchanges")]
    [Route("api/v{version:apiVersion}/exchanges")]
    [Authorize]
    public class ExternalExchangeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ExternalExchangeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<List<ExternalExchangeDTO>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetExternalExchanges()
        {
            var response = await _mediator.Send(new GetExternalExchangesQuery());
            return new SuccessResult(response);
        }
    }
}