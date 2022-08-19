using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Statistics.Queries.DailyRaisedCapital;
using Mosaico.Application.Statistics.Queries.RaisedFundsByCurrency;
using Mosaico.Application.Statistics.Queries.TopInvestors;

namespace Mosaico.API.v1.Statistics.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("api/projects")]
    [Route("api/v{version:apiVersion}/projects")]
    public class SaleStatisticsController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public SaleStatisticsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpGet("{id}/statistics/sale/top-investors")]
        [ProducesResponseType(typeof(SuccessResult<TopInvestorsResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetVisitStatistics([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new TopInvestorsQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{id}/statistics/sale/daily")]
        [ProducesResponseType(typeof(SuccessResult<DailyRaisedCapitalResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DailyRaisedCapital([FromRoute] Guid id, [FromQuery] int monthsAgo)
        {
            var response = await _mediator.Send(new DailyRaisedCapitalQuery
            {
                FromMonthAgo = monthsAgo,
                ProjectId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{id}/statistics/sale/by-currency")]
        [ProducesResponseType(typeof(SuccessResult<RaisedFundsByCurrencyResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RaisedFundsByCurrency([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new RaisedFundsByCurrencyQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
    }
}