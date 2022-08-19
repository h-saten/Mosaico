using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Statistics.Queries.StatisticsSummary;

namespace Mosaico.API.v1.Statistics.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("api/projects")]
    [Route("api/v{version:apiVersion}/projects")]
    public class StatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public StatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{id}/statistics/summary")]
        [ProducesResponseType(typeof(SuccessResult<StatisticsSummaryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVisitStatistics([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new StatisticsSummaryQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
    }
}