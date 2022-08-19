using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mosaico.API.Base.Responses;
using Mosaico.Application.Wallet.Queries.GetActiveBlockchains;
using Mosaico.Application.Wallet.Queries.GetDeploymentEstimate;
using Mosaico.Integration.SignalR.DTO;

namespace Mosaico.API.v1.Wallet.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/chains")]
    [Route("api/v{version:apiVersion}/chains")]
    [AllowAnonymous]
    public class BlockchainController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BlockchainController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(SuccessResult<GetActiveBlockchainsQueryResponse>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetActiveBlockchains()
        {
            var response = await _mediator.Send(new GetActiveBlockchainsQuery());
            return new SuccessResult(response);
        }
        
        [HttpGet("estimates")]
        [ProducesResponseType(typeof(SuccessResult<List<DeploymentEstimateDTO>>), StatusCodes.Status200OK)]
        [AllowAnonymous]
        public async Task<IActionResult> GetDeploymentEstimate()
        {
            var response = await _mediator.Send(new GetDeploymentEstimateQuery());
            return new SuccessResult(response);
            
        }
    }
}