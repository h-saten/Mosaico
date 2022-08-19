using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.DTO;
using Mosaico.Application.Wallet.Queries.GetSalesAgents;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/agents")]
    [Route("api/v{version:apiVersion}/agents")]
    [Authorize]
    public class SalesAgentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesAgentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<List<SalesAgentDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSalesAgents([FromQuery] string company = null)
        {
            var query = new GetSalesAgentsQuery
            {
                Company = company
            };
            var response = await _mediator.Send(query);
            return new SuccessResult(response);
        }
    }
}