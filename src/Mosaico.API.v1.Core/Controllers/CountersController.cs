using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Core.Queries.GetCounters;

namespace Mosaico.API.v1.Core.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/counters")]
    [Route("api/v{version:apiVersion}/counters")]
    
    public class CountersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CountersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetCountersQueryResponse>), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetCounters([FromQuery] GetCountersQuery command)
        {

            if (command == null)
                throw new InvalidParameterException(nameof(command));

            var response = await _mediator.Send(command);
            return new SuccessResult(response);
        }
    }
}