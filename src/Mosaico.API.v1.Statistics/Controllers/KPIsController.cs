using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Statistics.Queries.GetKPIs;

namespace Mosaico.API.v1.Statistics.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/kpis")]
    [Route("api/v{version:apiVersion}/kpis")]
    
    public class KPIsController : ControllerBase
    {
        private readonly IMediator _mediator;

        
        public KPIsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetKPIsQueryResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetKPIs()
        {
            var response = await _mediator.Send(new GetKPIsQuery());
            return new SuccessResult(response);
        }

    }
}