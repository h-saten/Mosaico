using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.BusinessManagement.Commands.CreateProposal;
using Mosaico.Application.BusinessManagement.Commands.Vote;
using Mosaico.Application.BusinessManagement.DTOs;
using Mosaico.Application.BusinessManagement.Queries.GetCompanyProposals;
using Mosaico.Authorization.Base;

namespace Mosaico.API.v1.BusinessManagement.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/dao/{id}/proposals")]
    [Route("api/v{version:apiVersion}/dao/{id}/proposals")]
    public class ProposalController : ControllerBase
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly IMediator _mediator;

        public ProposalController(ICurrentUserContext currentUserContext, IMediator mediator)
        {
            _currentUserContext = currentUserContext;
            _mediator = mediator;
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<List<ProposalDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<IActionResult> GetProposals([FromRoute] Guid id, [FromQuery] int take = 9, int skip = 0)
        {
            take = take <= 0 || take > 100 ? 9 : take;
            skip = skip < 0 ? 0 : skip;
            
            var response = await _mediator.Send(new GetCompanyProposalsQuery
            {
                CompanyId = id,
                Skip = skip,
                Take = take
            });
            return new SuccessResult(response);
        }

        [HttpPost]
        [ProducesResponseType(typeof(SuccessResult<List<ProposalDTO>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status403Forbidden)]
        [Authorize]
        public async Task<IActionResult> CreateProposalAsync([FromRoute] Guid id, [FromBody] CreateProposalCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.CompanyId = id;
            command.Title = command.Title?.Trim();
            command.Description = command.Description?.Trim();
            command.QuorumThreshold = 20;
            var result = await _mediator.Send(command);
            return new SuccessResult(result);
        }

        [HttpPut("{proposalId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status400BadRequest)]
        [Authorize]
        public async Task<IActionResult> VoteAsync([FromRoute] Guid id, [FromRoute] Guid proposalId, [FromBody] VoteCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.CompanyId = id;
            command.ProposalId = proposalId;
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}