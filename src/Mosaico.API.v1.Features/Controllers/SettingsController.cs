using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Features.Commands.UpsertSetting;
using Mosaico.Application.Features.Queries.GetAllEntitySettingsQuery;
using Mosaico.Application.Features.Queries.GetSetting;

namespace Mosaico.API.v1.Features.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/settings")]
    [Route("api/v{version:apiVersion}/settings")]
    public class SettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{category}/{name}")]
        [ProducesResponseType(typeof(SuccessResult<GetSettingQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetSetting([FromRoute] string name, [FromRoute] string category, [FromQuery] Guid? entityId = null)
        {
            var response = await _mediator.Send(new GetSettingQuery
            {
                Name = name,
                EntityId = entityId,
                Category = category
            });
            return new SuccessResult(response);
        }
        
        [HttpGet("{category}")]
        [ProducesResponseType(typeof(SuccessResult<GetSettingQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetSettings([FromRoute] string category, [FromQuery] Guid? entityId = null)
        {
            var response = await _mediator.Send(new GetAllEntitySettingsQuery
            {
                EntityId = entityId,
                Category = category
            });
            return new SuccessResult(response);
        }
    }
}