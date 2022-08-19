using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.Affiliation.ApplyUserReference;
using Mosaico.Application.ProjectManagement.Commands.Airdrop.CreateAirdrop;
using Mosaico.Application.ProjectManagement.Queries.Affiliation.GetUserAffiliation;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/affiliations")]
    [Route("api/v{version:apiVersion}/affiliations")]
    public class AffiliationController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserContext _currentUserContext;
        
        public AffiliationController(IMediator mediator, ICurrentUserContext currentUserContext)
        {
            _mediator = mediator;
            _currentUserContext = currentUserContext;
        }
        
        [HttpGet("self")]
        [ProducesResponseType(typeof(SuccessResult<GetUserAffiliationQueryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> GetUserAffiliation()
        {
            var response = await _mediator.Send(new GetUserAffiliationQuery
            {
                UserId = _currentUserContext.UserId
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("reference")]
        [ProducesResponseType(typeof(SuccessResult<GetUserAffiliationQueryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> Reference([FromBody] ApplyUserReferenceCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.UserId = _currentUserContext.UserId;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}