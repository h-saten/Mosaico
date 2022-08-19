using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.ProjectManagement.Commands.UpsertPaymentCurrency;
using Mosaico.Application.ProjectManagement.Commands.UpsertPaymentMethod;
using Mosaico.Application.ProjectManagement.Queries.GetProjectSaleDetails;
using Mosaico.Application.ProjectManagement.Queries.GetProjectStage;

namespace Mosaico.API.v1.ProjectManagement.Controllers
{
    public partial class ProjectController
    {
        [HttpGet("{id}/sale/report")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SuccessResult<GetProjectStageQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ProjectSaleReport([FromRoute] string id)
        {
            var result = await _mediator.Send(new GetProjectSaleDetailsQuery
            {
                UniqueIdentifier = id
            });

            return new SuccessResult(result);
        }
        
        [HttpPut("{id}/sale/payment-method")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SuccessResult<GetProjectStageQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpsertPaymentMethod([FromRoute] Guid id, [FromBody] UpsertPaymentMethodCommand command)
        {
            command.ProjectId = id;
            var result = await _mediator.Send(command);

            return new SuccessResult(result);
        }
        
        [HttpPut("{id}/sale/payment-currency")]
        [ProducesResponseType(typeof(SuccessResult<GetProjectStageQueryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpsertPaymentCurrency([FromRoute] Guid id, [FromBody] UpsertPaymentCurrencyCommand command)
        {
            command.ProjectId = id;
            var result = await _mediator.Send(command);

            return new SuccessResult(result);
        }
    }
}