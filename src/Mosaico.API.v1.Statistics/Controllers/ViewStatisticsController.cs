using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Statistics.Queries.VisitsPerDay;

namespace Mosaico.API.v1.Statistics.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("api/projects")]
    [Route("api/v{version:apiVersion}/projects")]
    public class ViewStatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public ViewStatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{id}/statistics/visits/daily")]
        [ProducesResponseType(typeof(SuccessResult<VisitsPerDayResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVisitDailyStatistics([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new VisitsPerDayQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
    }
}