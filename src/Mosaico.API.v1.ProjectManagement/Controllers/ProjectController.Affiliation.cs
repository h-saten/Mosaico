using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Exceptions;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.Affiliation.AddPartner;
using Mosaico.Application.ProjectManagement.Commands.Affiliation.DisablePartner;
using Mosaico.Application.ProjectManagement.Commands.Affiliation.EnablePartner;
using Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdatePartner;
using Mosaico.Application.ProjectManagement.Commands.Affiliation.UpdateProjectAffiliation;
using Mosaico.Application.ProjectManagement.Queries.Affiliation.GetAffiliation;
using Mosaico.Application.ProjectManagement.Queries.Affiliation.GetPartners;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        [HttpPost("{id}/affiliation")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdateAffiliation([FromRoute] Guid id, [FromBody] UpdateProjectAffiliationCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response) {StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpGet("{id}/affiliation")]
        [ProducesResponseType(typeof(SuccessResult<GetAffiliationQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetAffiliation([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetAffiliationQuery
            {
                ProjectId = id
            });
            return new SuccessResult(response);
        }
        
        [HttpPost("{id}/affiliation/partners")]
        [ProducesResponseType(typeof(SuccessResult<Guid>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> AddPartnerAsync([FromRoute] Guid id, [FromBody] AddPartnerCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command));
            command.ProjectId = id;
            var response = await _mediator.Send(command);
            return new SuccessResult(response) {StatusCode = StatusCodes.Status201Created};
        }
        
        [HttpGet("{id}/affiliation/partners")]
        [ProducesResponseType(typeof(SuccessResult<GetPartnersQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> GetPartnersAsync([FromRoute] Guid id, [FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            if (skip < 0) skip = 0;
            if (take <= 0) take = 10;
            var response = await _mediator.Send(new GetPartnersQuery
            {
                ProjectId = id,
                Skip = skip,
                Take = take
            });
            return new SuccessResult(response);
        }
        
        [HttpPut("{id}/affiliation/partners/{partnerId}")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> UpdatePartner([FromRoute] Guid id, [FromRoute] Guid partnerId, [FromBody] UpdatePartnerCommand command)
        {
            if (command == null) throw new InvalidParameterException(nameof(command), "Invalid command");
            command.ProjectId = id;
            command.PartnerId = partnerId;
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPut("{id}/affiliation/partners/{partnerId}/enable")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> EnablePartner([FromRoute] Guid id, [FromRoute] Guid partnerId)
        {
            var command = new EnablePartnerCommand
            {
                ProjectId = id,
                PartnerId = partnerId
            };
            await _mediator.Send(command);
            return new SuccessResult();
        }
        
        [HttpPut("{id}/affiliation/partners/{partnerId}/disable")]
        [ProducesResponseType(typeof(SuccessResult), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        [Authorize]
        public async Task<IActionResult> DisablePartner([FromRoute] Guid id, [FromRoute] Guid partnerId)
        {
            var command = new DisabledPartnerCommand
            {
                ProjectId = id,
                PartnerId = partnerId
            };
            await _mediator.Send(command);
            return new SuccessResult();
        }
    }
}